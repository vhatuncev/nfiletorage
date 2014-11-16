using System;

namespace IntegrationTests.Models
{
    public class TestObject
    {   
        public TestObject() {}
        public TestObject(string testString, int testInt, DateTime testDate)
        {
            TestString = testString;
            TestInt = testInt;
            TestDate = testDate;
        }

        public string TestString { get; set; }
        public int TestInt { get; set; }
        public DateTime TestDate { get; set; }
    }
}
