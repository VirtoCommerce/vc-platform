---
title: Sequences
description: Sequences
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 5
---
Sequences are required for unique number or id generation as an alternative to GUID. GUID is not human friendly format and is really difficult to say. Our system has an implementation of sequence generator so that it can produce unique sequence of string in a desired and easy to say format.

## Data structure

The database structure of sequence is very simple. Additional to common columns (LastModified, Created and Discriminator) there are only two columns: ObjectType(string) and Value(int). One row in database table represents a sequence for certain object type. Value is used to save the last reserved counter.

## Namespaces

* **VirtoCommerce.Foundation.AppConfig.Model.Sequence** - the database model for sequences
* **VirtoCommerce.Client.SequencesClient** - holds the actual implementation for generating sequences
* **VirtoCommerce.Web.Client.Services.Sequences.SequenceService** - web service that returns next item of given sequence, impements ISequenceService
* **VirtoCommerce.Foundation.Frameworks.Sequences.ISequenceService** - the interface for service

```
namespace VirtoCommerce.Foundation.Frameworks.Sequences
{
  [ServiceContract(Namespace = "http://schemas.virtocommerce.com/1.0/sequences/")]
  public interface ISequenceService
  {
    /// <summary>
    /// Gets the next number of sequence identified by type.
    /// </summary>
    /// <param name="fullTypeName">Full type name of the sequence used as key.</param>
    /// <returns>Unique string</returns>
    [OperationContract]
    string GetNext(string key);
  }
}
```

## How it works

The actual sequence are stored in memory and only reserves counter in database that is last used in memory. The counter is reset to zero when day changes, generated string contains date part in template. The changes to memory are done by locking static in-memory sequence object and changes to database are done using TransactionScope. When implementation is called to get next number following actions are taken:

* The next number is returned from memory when possible
* If memory is empty or has expired (last generation date is less then current date) new sequence is generated
* The start counter is taken from database value by given key and end counter is startCounter + [SequenceReservationRange]
  * Start counter is reset to zero if value in database has expired or
  * Start counter + [SequenceReservationRange] generated overflow exception
* New value is saved to database

Here is a sample code taken from SequenceClient:

```
public string GenerateNext(string objectType)
{
  lock (SequenceLock)
  {
    InMemorySequences[objectType] = InMemorySequences[objectType] ?? new InMemorySequence(objectType);
    if (InMemorySequences[objectType].IsEmpty || InMemorySequences[objectType].HasExpired)
    {
      var startCounter = 0;
      var endCounter = SequenceReservationRange;
      //Update Sequences in database
      using (var transaction = new TransactionScope())
      {
        var sequence = _repository.Sequences.SingleOrDefault(s => s.ObjectType.Equals(objectType, StringComparison.OrdinalIgnoreCase));
        if (sequence != null)
        {
          startCounter = sequence.Value;
          //Sequence in database has expired?
          if (sequence.LastModified.HasValue && sequence.LastModified.Value.Date < DateTime.Now.Date)
          {
            startCounter = 0;
          }
          try
          {
            endCounter = checked(startCounter + SequenceReservationRange);
          }
          catch (OverflowException)
          {
            //need to reset
            startCounter = 0;
            endCounter = SequenceReservationRange;
          }
        }
        else
        {
          sequence = new Sequence { ObjectType = objectType };
          _repository.Add(sequence);
        }
        //Commit changes
        sequence.Value = endCounter;
        _repository.UnitOfWork.Commit();
        //Transaction success
        transaction.Complete();
      }
      //Pregenerate
      InMemorySequences[objectType].Pregenerate(startCounter, endCounter);
    }
    return string.Format(InMemorySequences[objectType].Next());
  }
}
```

## In-Memory sequence

The object used to store sequences in memory. The main method here is Pregenerate that takes start and end index. It uses template to generate sequence which is in desired format. Below is the method sample that generated sequence.

```
public void Pregenerate(int startCount, int endCount)
{
  var generatedItems = new Stack<string>();
  for (var index = startCount; index < endCount; index++)
  {
    var strCount = index.ToString(CultureInfo.InvariantCulture).PadLeft(CounterLength, '0');
    generatedItems.Push(string.Format(IdTemplate, _prefix, DateTime.Now.ToString(DateFormat), strCount));
  }
  //This revereses the sequence
  _sequence = new Stack<string>(generatedItems);
  _lastGenerationDateTime = DateTime.Now;
}
```

For example lets say that these are the settings and we want to generate TrackingNumber for Order:

```
public const string IdTemplate = "{0}{1}-{2}";
//How many sequence items will be stored in-memory
public const int SequenceReservationRange = 100;
//Constant length of counter. Trailing zeros are added to left.
public const int CounterLength = 5;
public const string DateFormat = "yyyy-MMdd";
```

Then sequence generated will look similar to:

ORDER2013-0522-00000

ORDER2013-0522-00001

...

ORDER2013-0522-00100
