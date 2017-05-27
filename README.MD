# XmlConsumer.NET

## Introduction

XmlConsumer.NET is a domain-specific language designed to easily extract data from an XML document into a .NET object.
This is a mapping process that we call **consumption** at a high level and **loading** at the low level.

It is ideal for .NET applications that must populate the same object model but receive different XML formats such as a web service or a messaging endpoint.

To use it, you embed the interpreter component into your application and specify 3 things:

1. The **Source XML Document**, or just **Source**.  This is the XML document you are extracting data from.

1. The **Target Object**, or just **Target**.  This is the instance of the .NET object you are populating, or **loading**.

1. The **Mapping Script**, or just **Script**.
This is the set of Xpath expressions combined with a terse and powerful syntax that defines how the Target is loaded.
The language of the Script is not-very-creatively named **XML Consumer Mapping Script**, or **XCMS**.

## Motivation

As an integrations developer at a leading background check company - each customer had its own XML message format for ordering our services.
We had hundreds of customers - so there was a huge variety of XML data formats that we had to process.
We had the following recurring problems:

1. We needed to extract hundreds of data points from an XML message.

0. The data points had to be placed into the same well-defined object model that all integration applications used.

0. The data points we needed to collect changed often.  Also, how we needed to collect a data point - such as where it would go - changed often.

0. Applying a single change the mapping logic was a slow process.  We had to recompile and redeploy the .NET assemblies.

## Solution

XmlConsumer.NET solves these problems by offering the following major benefits:

1. All XPath expressions and node set processing logic are factored out into an XCMS script.
This makes your application shorter because you do not have to write logic that invokes the XPath interpreter and loops through the results.

1. An XCMS script is designed to declaratively pair a target property or field with a source XPath expression.
The interpreter uses .NET reflection to find the members of the Target - so it can work with any object.

1. Changing where a source data point goes is as simple changing the target member name in the XCMS script - which is just a text file.
XCMS scripts are designed to be terse - so it is also easy to identify where a change needs to be made in little time.

1. Changing the XCMS script does not imply that your application will need to be recompiled.  This makes development faster during the change/test cycle.

1. From a deployment perspective, applying mapping process changes are a configuration concern.
Replacing the mapping script has an immediate effect.
Your application does not need to be restarted.

If you choose to take advantage of them, there are additional minor benefits, too:

1. Direct "bytes to object" load strategy.
You can specify the encoding to use in the XCMS script and have the interpreter load the source document for you, directly from a stream or array of bytes.

1. Automatic Instantiation.
Instead of creating and providing the target yourself, you can just provide the type of the object and have the interpreter create an empty instance for you.

## Getting Started

As an example, let's pretend that we are a background check company.
We have a client that orders our services by sending us an XML message that looks like this:

	<?xml version="1.0"?>
	<ClientMessage>
		<DepartmentName>Information Technology</DepartmentName>
	</ClientMessage>

We have a background check class defined as follows:

	public class BackgroundCheck
	{
		public string BillingCode;
	}
	
We need to save the client's department as a billing code for downstream reporting.

Therefore, we want the 'BillingCode' field on the target object to have the value of the 'DepartmentName' element in the source.

We can express this concisely in an XCMS script by writing:

	{
		BillingCode: 'child::DepartmentName';
	}

There are several things that we just expressed here:

* Implicitly, the interpreter has set the default expression type to be **XPath**.

* Implicitly, the interpreter has made the **source context element** for all XPath expressions to be the root element of the source document.
In this example, this is the 'ClientMessage' element.

* An opening curly brace '{' means we are entering a new **target object context**.
In this example this is just the instance of the 'BackgroundCheck' provided to the interpreter.

* On the left side of the ':' is the name of the **target field** on the .NET object.
This means we are targeting the 'BillingCode' field on the target object.
This is what gets its value set.

* On the right side of the ':' is an XPath expression.
This XPath expression gets the value of the 'DepartmentName' child element.
In general, this is a type of **mapping statement**.

* The closing curly brace '}' means we are exiting the current **target object context**.
This means we are done changing the members of the current object.
In this example, this is just the instance of the 'BackgroundCheck' provided to the interpreter.

You may notice that XCMS looks like a combination of CSS and JSON.

Now let's look at what we have to write to perform this mapping in your application.
Start by referencing the XmlConsumer Namespace:

	using OpCheck.XmlConsumer;
	
	XmlConsumerInterpreter CreatedInterpreter = new XmlConsumerInterpreter();
	CreatedInterpreter.Init();
	CreatedInterpreter.SourceDocument = SourceXmlDocument;
	CreatedInterpreter.ScriptFilePath = ScriptFilePath;
	CreatedInterpreter.TargetObject = new BackgroundCheck();
	CreatedInterpreter.Execute();

//
// CREATE AND INITIALIZE THE INTERPRETER.
// SPECIFY THE SOURCE XML DOCUMENT - AN INSTANCE OF XmlDocument.  THIS IS THE DOCUMENT THAT YOU ARE CONSUMING.
// SPECIFY THE SCRIPT FILE PATH THAT CONTAINS THE MAPPING LOGIC.
// SPECIFY THE TARGET OBJECT THAT YOU ARE POPULATING.
// EXECUTE THE MAPPING AND POPULATE THE TARGET OBJECT.
//
	
## Mapping File Basics

The mapping file contains the logic that reads the XML document and populates the target object.
A mapping file consists of an "options tuple" followed by a series of mapping statements.

The following is an example of a mapping file that reads the department from the source XML document and writes it to the billing code field on the target object:

	//
	// THE OPTIONS TUPLE IS CONTAINED IN A PAIR OF PARENTHESES.
	//
	(
		ExpressionType: 'XPath';
		Binding: 'self::*';
	)
	{
		//
		// MAPPING STATEMENTS GO HERE.
		// THE LEFT SIDE IS THE NAME OF THE FIELD OR PROPERTY ON THE TARGET OBJECT.
		// THE RIGHT SIDE IS AN XPATH STATEMENT.
		//
		BillingCode: 'child::BackgroundCheckOrder/child::Department';
	}

From the above example, take note of the following:

* The options tuple is contained in parentheses: '(', ')'.

* The mapping statements are contained by curly braces: '{', '}'.

* Options and mapping statements can end with a semicolon: ';'.  Sometimes this is required and other times it is optional.  We'll cover both cases below.

The options tuple is not required.  If you exclude it, then the defaults for the interpreter are used.

	//
	// NO OPTIONS TUPLE SPECIFIED.  THE INTERPRETER DEFAULTS ARE USED.
	//
	{
		BillingCode: 'child::BackgroundCheckOrder/child::Department';
	}

It's also OK to write an empty mapping statement set:

	{
		//
		// THIS MAPPING STATEMENT SET IS EMPTY.
		// THE INTERPRETER WILL NOT MAP ANYTHING AND THE TARGET OBJECT IS NOT CHANGED AT ALL.
		//
	}

You may have already noticed that single-line comments start with two forward slashes:

	//
	// THIS IS A COMMENT LINE.
	//

	


	{
		BillingCode1: 'BillingCode1';
	}


	(
		ExpressionType: 'XPath';
		Encoding: 'UTF8';
		Binding: 'self::*';
	)
	{
		BillingCode2: 'BillingCode2';
	}

