---
title: Using dynamic properties
description: Developer guide to using dynamic properties
layout: docs
date: 2015-09-04T00:04:01.910Z
priority: 4
---
## Overview

VirtoCommerce Platform allows for the addition of new properties toВ entities at runtime.

Entity should implementВ IHasDynamicProperties interface.

```
public interface IHasDynamicProperties
{
    string Id { get; }
    string ObjectType { get; set; }
    ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
}
```

When loading, saving and deleting an entity you should call corresponding IDynamicPropertyService methods and pass the entity.В TheВ DynamicPropertyService will process the passed entity and its child entities, which implementВ IHasDynamicProperties interface.

## Managing dynamic properties

### UsingВ IDynamicPropertyService

```
public void CreateDynamicProperties(IDynamicPropertyService dynamicService)
{
    var properties = new[]
    {
        new DynamicProperty
        {
            ObjectType = "Product",
            ValueType = DynamicPropertyValueType.Boolean,
            Name = "Is On Sale",
        },
        new DynamicProperty
        {
            ObjectType = "Product",
            ValueType = DynamicPropertyValueType.Decimal,
            Name = "Sale Price",
        },
    };
    dynamicService.SaveProperties(properties);
}
```

### Using UI

See the [Managing dynamic properties](docs/vc2userguide/configuration/managing-dynamic-properties)В article in the User Guide section.

## Example

In the following example we enable dynamic properties for MyEntity by implementing theВ IHasDynamicProperties and use MyEntityServiceВ for working with these entities.

```
using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.DynamicProperties;
 
namespace MyModule
{
    public class Tests
    {
        public void UseDynamicProperties(IDynamicPropertyService dynamicService)
        {
            var entityService = new MyEntityService(dynamicService);
            var entity = entityService.Create(new MyEntity());
            var value = entity.GetDynamicPropertyValue("Price", 0m);
        }
    }
    public class MyEntity : IHasDynamicProperties
    {
        public string Id { get; set; }
        public string ObjectType { get; set; }
        public ICollection<DynamicObjectProperty> DynamicProperties { get; set; }
    }
    public class MyEntityService
    {
        private readonly IDynamicPropertyService _dynamicPropertyService;
 
        public MyEntityService(IDynamicPropertyService dynamicPropertyService)
        {
            _dynamicPropertyService = dynamicPropertyService;
        }
 
        public MyEntity GetById(string id)
        {
            var entity = LoadEntityFromDatabase(id);
            _dynamicPropertyService.LoadDynamicPropertyValues(entity);
            return entity;
        }
 
        public MyEntity Create(MyEntity entity)
        {
            SaveEntityToDatabase(entity);
            _dynamicPropertyService.SaveDynamicPropertyValues(entity);
            return GetById(entity.Id);
        }
 
        public void Update(MyEntity entity)
        {
            SaveEntityToDatabase(entity);
            _dynamicPropertyService.SaveDynamicPropertyValues(entity);
        }
 
        public void Delete(string id)
        {
            var entity = GetById(id);
            _dynamicPropertyService.DeleteDynamicPropertyValues(entity);
        }
 
 
        private MyEntity LoadEntityFromDatabase(string id)
        {
            throw new NotImplementedException();
        }
 
        private void SaveEntityToDatabase(MyEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
```
