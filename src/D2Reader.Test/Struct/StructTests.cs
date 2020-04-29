using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.InteropServices;
using Zutatensuppe.D2Reader.Struct;

namespace Zutatensuppe.DiabloInterface.D2Reader.Test.Struct
{
    [TestClass]
    public class StructTests
    {
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
                var fieldOffset = (uint)Marshal.OffsetOf(type, field.Name).ToInt32();
                Assert.AreEqual(attribute.Offset, fieldOffset,
                    $"[{type.Name}.{field.Name}] offset mismatch!");
            }
        }

        /// <summary>
        /// Get all types has field with the ExpectOffset attribute.
        /// </summary>
        /// <returns>The types with the attribute.</returns>
        static IEnumerable<Type> EnumerateStructTypes()
        {
            var attributeType = typeof(ExpectOffsetAttribute);
            return from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from type in assembly.GetTypes()
                where type.GetFields().Any(field => Attribute.IsDefined(field, attributeType))
                select type;
        }
    }
}
