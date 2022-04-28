# TODO

## Current Actions

- Follow source generation @ https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview

## Ideas

- Refactor g3d serialization & deserialization to leverage code generation techniques to improve maintenance.
    - .net6.0 code gen -> reference code in .netstandard2.0|net6.0 etc project.
    - I want to define my attributes and have the serializer/deserializer written for me.
    - For that, the g3d repo should provide a code gen project which reads attribute definitions and emits code to read those attributes from a stream.

## Abstractions

- g3d is a container of attributes
    - g3d has a rigid structure:
        - header
        - unordered attributes

- vim.g3d is a specific collection of attributes
    - instances
    - meshes
    - 
    - shapes

