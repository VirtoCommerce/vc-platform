---
title: Release Management Process
description: The article about Virto Commerce release management process
layout: docs
date: 2016-02-25T19:34:39.310Z
priority: 1
---
As with any enterprise software solution, you should follow established software release management guidelines when you develop and release a <a class="crosslink" href="https://virtocommerce.com/b2b-ecommerce-platform" target="_blank">Virto Commerce</a> solution. This process should include the following distinct stages:
* Development
* Testing
* Staging
* Production

Ideally, you should complete each stage in the release management process in a discrete environment, separate from the other environments. Realistically, you may have to combine one or more of the environments due to hardware, time, or other resource constraints. At a bare minimum, you should separate the production environment from the other environments.

## Development Environment

We recommend that developers have their own development computer (physical or virtual) with the necessary software installed. Therefore, we assumed that the first stage of release management process is local development environment.

The second stage is traditional development environment. This is the working environment for individual developers or small teams, where they commit code changes. We recommend that this stage will be an environment that is a limited version of the production environment.

## Testing Environment

The goal of this environment is to combine and validate the work of the entire project team so it can be tested before being promoted to the staging environment.

On this stage the various testing methodologies should be completed before deploying the solution to next stage. This phase is critical and must be extensively completed to avoid possible issues later.

## Staging Environment

The staging environment is an environment that is as identical to the production environment as possible. The purpose of the staging environment is to simulate as much of the production environment as possible.

## Production Environment

The production environment is the "live" environment that will host the running Virto Commerce solution. The production environment is the final endpoint in the release management process and should host only Virto Commerce applications that have previously undergone development, unit testing, load testing, and staging in the other environments. Thorough unit testing, integration testing with physical devices, and staging beforehand will help to ensure fewer operational issues and achieve maximum performance in the production environment.
