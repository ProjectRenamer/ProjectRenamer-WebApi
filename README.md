# ProjectRenamer-WebApi

[![Build Status](https://travis-ci.org/ProjectRenamer/ProjectRenamer-WebApi.svg?branch=master)](https://travis-ci.org/ProjectRenamer/ProjectRenamer-WebApi)
[![Docker Pulls](https://img.shields.io/docker/pulls/projectrenamer/projectrenamer-webapi.svg)](https://hub.docker.com/r/projectrenamer/projectrenamer-webapi)

This project change given key word to another given word for all file and file-content under given .git repoitory.

You can reach docker-container over [this link](https://hub.docker.com/r/projectrenamer/projectrenamer-webapi/)

You can reach service over [this link](https://project-renamer-api.azurewebsites.net)

## Using

This WebApi contains 2 endpoint under "ProjectGenerator" controller. One of them is "generator" and another one is "download".

First of all, you have to give project name, repoitory url, branch name (default master) and key-value pair list. If repository is open, you do not have to give username and password.

![generator request](https://preview.ibb.co/jPzrA7/generator.png)

If every thing is going well, generator end-point return token value for you. You give this token to "download" FileResultContent return for you.

![generator request](https://image.ibb.co/eNMhHn/download.png)


Also may be you want to take a look [ProjectRenamer-UI](https://github.com/ProjectRenamer/ProjectRenamer-UI)
