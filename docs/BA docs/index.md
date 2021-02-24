# Overview

## Purpose

This document is created for Business Analysts to help them in mapping Virto Commerce functionality on the eCommerce project requirements, understand easily what is available out of the box and what can be extended with the built-in extensibility tools.

## BA documentation structure

Virto Commerce is a headless API-based eCommerce platform. It means that each and every action that is available in the admin panel is available in API, so it can be transmitted to the storefront. 

Some entities, properties, or functions are unavailable out-of-the-box, because they may vary from project to project. At the same time, it can be easily added by developers using Virto Commerce native extensibility points. These code extensions are update neutral: it means that the module updated from the vendor will be seamlessly available or will require small efforts from the project dev team.

<Add declarative extensibility>

The other way of extending Virto functionality is reactive programming. Since Virto Commerce modules have native integration with Azure Event Grid. This makes it possible to extend the module's business logic through the Logic Apps development that can be done without backend developers' engagements.

Atomic actions in the backend can be consolidated in a customer experience scenario on the business logic layer, represented currently by the XAPI module. The XAPI module returns GraphQL business API that can be used by front-end developers. Generic scenarios are built-in. In case a company requires specific business logic implementation, developers can extend the logic inside the XAPI module.

Due to the listed reasons, the documentation for Business Analysts has the following structure for each module.

* Module responsibility scope (in business terms)
* Atomic functions: what can be configured and what scenarios can be emulated manually in the backend
* Possible backend functionality extending: 
    * Through the code extensibility
    * With the reactive programming
* User scenarios that are built-in into business API
* Examples of custom user scenarios that can be implemented in Virto XAPI by developers

## How to work with the documentation
The following flow is expected

1. A Business Analyst and the team defines the desirable customer journey scenarios that are expected to be implemented

2. The Business Analysts find the appropriate step in the documentation

3. The Business Analysts identifies how native functionality and extensibility covers scenarios

4. The output of the previous step is used to identify the configuration and development scope of the development iteration