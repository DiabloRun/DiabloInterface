using System;
using System.Linq;
using System.Collections.Generic;
using DiabloInterface.D2.Struct;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.InteropServices;

namespace tests
{
    [TestClass]
    public class StructTests
    {
        /// <summary>
        /// Get all types has field with the ExpectOffset attribute.
        /// </summary>
        /// <returns>The types with the attribute.</returns>
        public IEnumerable<Type> EnumerateStructTypes()
        {
            Type attributeType = typeof(ExpectOffsetAttribute);
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                   from type in assembly.GetTypes()
                   where type.GetFields().Any(field => Attribute.IsDefined(field, attributeType))
                   select type;
        }

        [TestMethod]
        public void EnsureOffsetsAreCorrect()
        {
            // Create a list of test tuples, one for each field with a ExpectOffset atribute.
            var testData = from type in EnumerateStructTypes()
                           from field in type.GetFields()
                           let attribute = field.GetCustomAttribute<ExpectOffsetAttribute>()
                           where attribute != null
                           select Tuple.Create(type, field, attribute);

            foreach (var testCase in testData)
            {
                var type = testCase.Item1;
                var field = testCase.Item2;
                var attribute = testCase.Item3;

                // Check if the real and expected offsets match.
                uint fieldOffset = (uint)Marshal.OffsetOf(type, field.Name).ToInt32();
                Assert.AreEqual(attribute.Offset, fieldOffset,
                    string.Format("[{0}.{1}] offset mismatch!", type.Name, field.Name));
            }
        }
    }
}
