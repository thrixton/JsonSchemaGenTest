﻿using NJsonSchema;
using System;

namespace JsonSchemaGenTest
{
    class Program
    {
        static void Main(string[] args)
        {

            /*
             * Test project to illustrate how to manually generate a JSON schema
This is our test json which should validate against our schema

[
    {
        "id":1,
        "value":"one",
        "enabled":false
    },
    {
        "id":2,
        "value":"two",
        "enabled":true
    }
]

This is what our schema should look like
{
    "$schema": "http://json-schema.org/draft-06/schema#",
    "type": "array",
    "items": {
        "$ref": "#/definitions/DataRowElement"
    },
    "definitions": {
        "DataRowElement": {
            "type": "object",
            "additionalProperties": false,
            "properties": {
                "id": {
                    "type": "integer"
                },
                "value": {
                    "type": "string"
                },
                "enabled": {
                    "type": "boolean"
                }
            },
            "required": [
                "enabled",
                "id",
                "value"
            ],
            "title": "DataRowElement"
        }
    }
}
             */


            var testJson = @"[
    {
        ""id"":1,
        ""value"":""one"",
        ""enabled"":false
    },
    {
        ""id"":2,
        ""value"":""two"",
        ""enabled"":true
    }
]";

            var testSchema = JsonSchema.FromSampleJson(testJson);

            var testSchemaJson = testSchema.ToJson();
            Console.WriteLine(testSchemaJson); 

            var objectSchema = new JsonSchema();
            objectSchema.Type = JsonObjectType.Object;
            objectSchema.Title = "DataRowElement";
            objectSchema.Properties["id"] = new JsonSchemaProperty { Type = JsonObjectType.Integer };
            objectSchema.Properties["value"] = new JsonSchemaProperty { Type = JsonObjectType.String };
            objectSchema.Properties["enabled"] = new JsonSchemaProperty { Type = JsonObjectType.Boolean };

            var schemaJson = objectSchema.ToJson();
            Console.WriteLine(schemaJson);
            Console.WriteLine("All good so far, this is our reference schema for our array elements");

            var arraySchema = new JsonSchema();
            arraySchema.Type = JsonObjectType.Array;
            arraySchema.Definitions["DataRowElement"] = objectSchema;

            // Aha, now we set the "item" of our outer array schema to a reference to our inner schema
            arraySchema.Item = new JsonSchema { Reference = objectSchema };

            schemaJson = arraySchema.ToJson();
            Console.WriteLine(schemaJson);

            Console.WriteLine("Success");

            Console.WriteLine("Any key to exit");
            Console.ReadKey();
        }
    }
}
