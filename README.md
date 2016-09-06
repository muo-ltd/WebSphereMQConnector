# WebSphere MQ Connector Sample

This is a sample WebSphere MQ Connector (Version 8). It uses IKVM generated version of the IBM assemblies to 
connect to the queue which prevents the need to install the IBM Client on the machine using it. The purpose
of this is to show how to do this, not to give a complete client. 

## Pre-requisites 

To run Websphere MQ the simplest way is to install Docker. 
* Windows 10 - enable Hype-V and then install [Docker For Windows](https://docs.docker.com/docker-for-windows/).
* Windows other - make sure Hyper-V is not installed and then Install [Docker Toolbox](https://www.docker.com/products/docker-toolbox)
* Linux install docker
* Mac - install [Docker for mac](https://docs.docker.com/engine/installation/mac/)

## Running Websphere MQ 

The Docker image is already pre-configured with a sample channel name and queue. The queues are all read/write so that you can view
the message using the [IBM Websphere MQ Explorer](http://www-01.ibm.com/software/integration/wmq/explorer/). 

To run:

~~~~
    docker-compose up -d 
~~~~

To stop:

~~~~
    docker-compose down
~~~~

| Infomation | Data |
| --- | --- |
| Host | Docker for Windows: localhost |
|      | Docker Toolkit : docker-machine ip | 
| Channel | TEST.CHANNEL |
| Queue | TEST.QUEUE |

## .Net Client

To use the IBM client from .Net you need to install the client on every machine. This makes the deployment more complicated and painfull. 
However it is possible to obtain the jar files required to the java client from IBM and then use IKVM to wrap them so that they can be called from 
.Net. Instructions of how to do this are below. The client code and a test console app are in the MqClient directory. 

## How to use IKVM to create independant MQ Client from IBM Sources

* Obtain the Jar file described in this [article](http://www-01.ibm.com/support/docview.wss?uid=swg21683398&myns=swgws&mynp=OCSSFKSJ&mync=en)
* Extract using java -jar <downloaded zip name>, accepting the licence
* Download IKVM from https://www.ikvm.net/download.html
* Add IKVM location \bin to your path
* cd into the directory that the jar extracted to and go to the JavaSE directory
* run the following command: ikvmc -target:library -sharedclassloader { com.ibm.mq.allclient.jar } { com.ibm.mq.traceControl.jar } { fscontext.jar } { jms.jar } { JSON4J.jar } { providerutil.jar }
* Copy the generated .dll files into a lib directory. Also copy the following IKVM Assemblies (IKVM.OpernJDK) Beans, Core, Management, Misc, Naming, Security, Util, Runtime
* Add references from your client code

## Links
Useful Links

* [MQ Security](https://www.ibm.com/developerworks/community/blogs/messaging/entry/getting_going_without_turning_off_mq_security?lang=en)
* [MQ using IKVM](http://stackoverflow.com/questions/9339062/how-to-get-connect-with-ibm-websphere-mq-by-using-c-net)
