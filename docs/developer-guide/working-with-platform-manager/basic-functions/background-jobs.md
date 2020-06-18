---
title: Background jobs
description: Background jobs are based on Hangfire library which allows to schedule execution of long running tasks on specific time or just start them in a fire-and-forget manner
layout: docs
date: 2015-06-20T00:42:14.007Z
priority: 1
---
## Overview

Background jobs are based on [Hangfire](http://hangfire.io/) library which allows to schedule execution of long running tasks on specific time or just start them in a fire-and-forget manner. Hangfire supports persistence, monitoring, execution history and automatic retries for failed jobs.

Each job is defined as a static or instance method of some public class. ItвЂ™s not required to implement any interface to define a job.

## Fire-and-forget tasks

These jobs are being executed only once and almost immediately after they fired.

```
BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));
```

## Recurring tasks

Recurring jobs are fired many times on the specified CRON schedule.

```
RecurringJob.AddOrUpdate(() => Console.WriteLine("Hello, world!"), Cron.Daily);
```

## Instance methods

Before executing an instance methodВ Hangfire willВ create an instance of the class resolving constructor arguments through Unity container.

```
public class EmailService
{
    public void Send() { }
}

BackgroundJob.Enqueue<EmailService>(x => x.Send());
```

## Method arguments

Method arguments are evaluated and serialized when the task is sheduled and not when the task is executed.

In the following example the job will write the same time every minute.

```
RecurringJob.AddOrUpdate(() => Console.WriteLine(DateTime.Now), Cron.Minutely);
```

## Cancellation tokens

Hangfire canВ notify jobs that application is shutting down or job is being canceled. One of the job's arguments should be IJobCancellationToken.В When sheduling such jobs you can pass null as cancellation token and Hangfire will pass the correct instance at execution time.

```
public static void Method(int iterationCount, IJobCancellationToken token)
{
    for (var i = 0; i < iterationCount; i++)
    {
        token.ThrowIfCancellationRequested();
        Thread.Sleep(1000);
    }
}

BackgroundJob.Enqueue(() => Method(Int32.MaxValue, null));
```
