# Overview

## Purpose

The purpose of BA documentation is to help Business Analysts to map Virto Commerce functionality on an eCommerce project requirements, understand easily what is available out-of-the-box and what can be extended with the built-in extensibility tools.

## How to work with the documentation

The following flow is expected
* A Business Analyst together with the team defines the desirable customer journey scenarios that are expected to be implemented.
* The Business Analyst finds the appropriate customer journey step and backoffice scenario in BA documentation.
* The Business Analyst identifies how native functionality and extensibility options cover scenarios that are required for the customer journey step.
* Based on the collected information the project team identifies the required configuration and development scope and efforts.

## BA documentation structure

### Customer journey step

The documentation for Business Analysts has the following structure for each customer journey step:

* Customer scenarios that are built-in into experience API
* Examples of complex custom user scenarios with an explanation of how it can be build based on atomic backend functions (see below) and experience API.

### Backoffice scenarios

* Overview: main entities, their relations, business context.
* Atomic functions: what can be configured and what scenarios can be managed manually in the backend.
* Recommendations on Virto Commerce extensibility:
  * Declarative extensibility: extending the functionality through the admin panel, without any developer's engagement
  * Reactive programming extensibility, that requires minimum effort from developers on the Virto side: events, webhooks
  * Native extensibility: adding custom modules

## Headless solution specific

It is required to remember, that Virto Commerce is a headless and API-based B2B eCommerce platform. It means that: 
* Each and every action that is available in the admin panel is available in API, so it can be transmitted to the storefront. 
* Developers have absolute freedom of business scenarios implementation in the storefront; at the same time, it means that there is no ready out-of-the-box UI.

## Extensibility explanation

Some entities, properties, or functions are unavailable out-of-the-box, because they may vary from project to project. At the same time, they can be easily added by developers due to Virto Commerce's native extensibility points.

These code extensions are update neutral: it means that updates from the vendor can be applied to the module seamlessly, without risks, and with a small effort from the project dev team.

The other way of extending Virto functionality is reactive programming. Virto Commerce modules have native integration with Azure Event Grid. This makes it possible to extend the module's business logic through the Logic Apps development that can be done without backend developers' engagements.