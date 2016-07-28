# LevelUp.Serializer
[![Build status](https://ci.appveyor.com/api/projects/status/cssaqqa7sqlmbah0?svg=true)](https://ci.appveyor.com/project/larrynung/levelup-serializer)

Feature
* Ease of use
* Supports almost all serializer, like Binary、Xml、Soap、Json、DataContract.
* Support serialize to file、serialize to stream、deserialize from file、deserialize from stream.
* Support Xml encryption.
* Support serialize accelerate through the XML serialization assemble.
 

Example
```C#
using System;
using System.Runtime.Serialization;
using LevelUp.Serializer;

namespace ConsoleApplication46
{
    [DataContract]
    [Serializable]
    public class Person
    {
        [DataMember]
        public string Name { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var larry = new Person()
            {
                Name = "Larry Nung"
            };

            //Xml
            var xml = Serializer.SerializeToText(larry, SerializerType.Xml);

            Console.WriteLine(xml);


            xml = larry.ToXML();

            Console.WriteLine(xml);


            larry = Serializer.DeSerializeFromText<Person>(xml, SerializerType.Xml);

            Console.WriteLine(larry.Name);


            //JSON
            var json = Serializer.SerializeToText(larry, SerializerType.Json);

            Console.WriteLine(json);


            json = larry.ToJSON();

            Console.WriteLine(json);


            larry = Serializer.DeSerializeFromText<Person>(json, SerializerType.Json);

            Console.WriteLine(larry.Name);


            //SOAP
            var soap = Serializer.SerializeToText(larry, SerializerType.Soap);

            Console.WriteLine(soap);

            
            larry = Serializer.DeSerializeFromText<Person>(soap, SerializerType.Soap);

            Console.WriteLine(larry.Name);
        }
    }
}
```

Link
----
* [NuGet Gallery | LevelUp.Serializer](https://www.nuget.org/packages/LevelUp.Serializer/)
* [larrynung/LevelUp.Serializer.JsonNet](https://github.com/larrynung/LevelUp.Serializer.JsonNet)
* [larrynung/LevelUp.Serializer.ProtobufNet](https://github.com/larrynung/LevelUp.Serializer.ProtobufNet)
* [larrynung/LevelUp.Serializer.Jil](https://github.com/larrynung/LevelUp.Serializer.Jil)
