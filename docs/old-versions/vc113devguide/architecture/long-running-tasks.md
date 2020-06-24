---
title: Long running tasks
description: Long running tasks
layout: docs
date: 2015-03-18T20:11:12.560Z
priority: 6
---
## Introduction

VirtoCommerce scheduler is the utility of executing periodical tasks in order to manage system data from background processes.В **Currently system needs those three tasks to run in background to ensure data consistency**:

1. Re-index catalog changes
2. Create index for new catalogs
3. Update orderвЂ™s status: pending to processed

You can create new jobs just by implementing IJobActivity interface. In order to inform В scheduler about new job, you should register implementation in the SystemJobs table.

<img src="../../../assets/images/table.jpg" />

There you can see the вЂњProcess Search Index WorkвЂќ registration information: job is implemented in the class named VirtoCommerce.Scheduling.Jobs.ProcessSearchIndexWork, starts periodically each 100 seconds; AllowMultipleInstance=1 means that job can be started on every server in cluster (AllowMultipleInstance=0 means that job can start only from one server, most often because such processes canвЂ™t run asynchronously).В Note: LastExecutionTime, NextExecution fields are unused and will be deleted soon.

You can use also VirtoCommerce manager form:

<img src="../../../assets/images/x3.png" />

## Scheduler on different platforms

There are differences in schedulers realization's for Azure and Windows Server platform:

| |Scheduler for Azure platform|Scheduler for Windows Server platform|
|-|----------------------------|-------------------------------------|
|вЂњHostвЂќ and realization|Azure Service hosts вЂњworker roleвЂќ<br />вЂњWorker roleвЂќ review jobs and decide which job it should launch|FrontEnd app (hosted in IIS) hosts Quartz scheduler in the background thread.<br />ScedulerHost web.config parameter should be set to вЂPrimaryвЂќ<br />Time is managed by Quartz intelligent processor; main features like вЂњdo not start one job when other is not finishedвЂќ are supported from the box.|
|вЂњHomeвЂќ assembly|Extensions\Jobs\AzureSchedulingLib|Extensions\Jobs\WindowsServerSchedulingLib|
|Scheduler persistence|In general scheduler do not resident in memory - scheduler is builded once in minute (WorkerRole tick) and save its state in the TaskSchedule table.|Scheduler is permanently in memory|
|Context|Directly use IJobContext|Context should be transformed to IJobExecutionContext (to Quartz task context)|

## Implementing Job

1. Implement IJobActivity interface
  ```
  public interface IJobActivity
  {
    bool Execute(IJobContext context);В В В  
  }
  ```
2. Use "IJobContext.TraceContext.Trace()" method for tracing.

Jobs doesn't have user interface so canвЂ™t report errors to the user directly.В You do not need to catch errors (system will do it for you) but if you need additional information then use tracing.В Each trace message contains correlation token - special guid - designed to easy reference all messages from "job started" til "job finished" events.

3. Job constructor should be created using DI pattern and VirtoCommerce model's factories and builder should be injected into the constructor.
  ```
  public class GenerateSearchIndexWork : IJobActivity
  {
    private readonly ISearchIndexController controller;
    public GenerateSearchIndexWork(ISearchIndexController controller)
    {
      this.controller = controller;
    }
В }
  ```
4. Job should be registered in the hostвЂ™s app Unity container, now it means to be mentioned in the hostвЂ™s application bootstrapper:
  ```
  container.RegisterType<GenerateSearchIndexWork>();
  ```

## JobвЂ™s execution audit

Job's results (start time, end time, success) are logged intoВ SystemJobLogEntry table.

<img src="../../../assets/images/x2.png" />

If Messages is null - the job execution was succesfull. Otherwise Message contains error information.

> Instance and multiinstance attributes are used only on Azure platform. Instance is an deployment instance name (you can think about it as about machine name in our cloud). Multiinstance is a flag that show that this task was called on many instances (all they should be in log separately with the same "taskScheduleId" but with different instance name values).

That how the same table looks in VirtoCommerce Manager (shows only last 500 rows):

<img src="../../../assets/images/x1.png" />

In case of error row will be highlighted with pink and the message will be shown as row's ToolTip

## JobвЂ™s execution trace

Each job reports its activity (start, stop, error) to the Windows Diagnostic event sourceВ  named вЂњVirtoCommerce.Sceduler.TraceвЂќ. You can manage trace level using standard System.Diagnostic options e.g. disabling activity logging by setting switchValue from All to Error:

```
<source name ="VirtoCommerce.ScheduleService.Trace" switchValue="Error">
```

However because of all events go throw VirtoCommerce ITraceContext context , there is the possibility to manage jobвЂ™s tracing individually.

This will disable activity reporting for all entities except GenerateSearchIndexWork.

First, include traceConfiguration section into configSections and set up trace context configuration that way:

```
<section name="traceContextConfiguration"
  type="LittleShrub.Common.Configuration.TraceContextConfigurationSection,В В В  LittleShrub.Common"В 
  allowDefinition="Everywhere" allowExeDefinition="MachineToApplication" restartOnExternalChanges="true"/>
<traceContextConfiguration>
  <context activity="false"/> <!-- default is false = do not report -->
  <context service="VirtoCommerce.Scheduling.Jobs.GenerateSearchIndexWork" activity="true"/> <!--overwrite default -->
</traceContextConfiguration>
```

## JobвЂ™sВ synchronizationВ between instances and SystemJobs table tracking

Multiinstance jobs are execued on all instances when singelton jobs (MultiInstance=false) are executed only on one instance in the Azure Cloud.В To support such behaviuor complex soluion was used. This is the principal schema of it:

<img src="../../../assets/images/clip1.jpg" />

There we can assume:

1. that вЂњfirst rowвЂќ threads starts вЂњin parallelвЂќ and never finish.
2. system is designed to create new queue and listener for every new job and remove the queue and the listener if job become disabled (e.g. from VC Manager). This is some kind of isolation.
3. singleton's job listener remove the message from the queue, when multiinstance listener left it for other instances
