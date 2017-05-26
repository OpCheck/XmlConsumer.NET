# XmlConsumer.NET

## Introduction

XmlConsumer.NET is a domain-specific language that accepts an XML document and populates a given .NET object.
This mapping process is called "consumption."

It is ideal for applications that must use the same data model but receive different XML formats - such as integration endpoints.

The application creates an instance of class XmlConsumerInterpreter and provides it a script and an object to populate.

## Motivation

As an integrations developer at a kickass background check company, I had the following recurring problem:

1. We needed to extract hundreds of data points from an XML message.

0. The data points had to be placed into a prescribed object model that all integrations used.

0. The data points we needed to collect would change.  Also, how we needed to collect a data point - such as where it would go - would change.

0. Changing the mapping logic was a slow process.  It required a recompile and redeployment of the assemblies.

## Solution

XmlConsumer.NET solves the last 4 problems by externalizing the mapping logic into a physical file.

* The lines of code that perform the mapping 

* You can now declaratively 

* During development, 

* In production, changing the mapping logic becomes a configuration issue.  All you have to do is replace the mapping script.

## Usage

	//
	// CREATE AND INITIALIZE THE INTERPRETER.
	//
	XmlConsumerInterpreter CreatedInterpreter = new XmlConsumerInterpreter();
	CreatedInterpreter.Init();
	
	//
	// SPECIFY THE SOURCE XML DOCUMENT - AN INSTANCE OF XmlDocument.  THIS IS THE DOCUMENT THAT YOU ARE CONSUMING.
	//
	CreatedInterpreter.SourceDocument = SourceXmlDocument;
	
	//
	// SPECIFY THE SCRIPT FILE PATH THAT CONTAINS THE MAPPING LOGIC.
	//
	CreatedInterpreter.ScriptFilePath = ScriptFilePath;
	
	//
	// SPECIFY THE TARGET OBJECT THAT YOU ARE POPULATING.
	//
	CreatedInterpreter.TargetObject = new BackgroundCheck();
	
	//
	// EXECUTE THE MAPPING AND POPULATE THE TARGET OBJECT.
	//
	CreatedInterpreter.Execute();

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

