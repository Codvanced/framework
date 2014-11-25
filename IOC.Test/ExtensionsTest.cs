using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using IOC.FW.Core;
using System.IO;
using System.Reflection;

namespace IOC.Test
{
    [TestFixture]
    public class ExtensionsTest
    {
        [Test]
        public void FisherYatesTest()
        {
            IList<ClassTest> list = new List<ClassTest>();

            for (int i = 0; i < 500; i++)
            {
                list.Add(new ClassTest
                {
                    ID = i + 1,
                    NAME = ((char)(i % 2 + 65)) + (i + 1).ToString()
                });
            }

            list.Shuffle();
            Assert.IsNotNull(list);

            var listCopyOne = list.FisherYates();
            Assert.IsNotNull(listCopyOne);

            var listCopyTwo = list.FisherYates(0, 100);
            Assert.IsNotNull(listCopyTwo);
        }

        [Test]
        public void HasElementsTest()
        {
            IList<ClassTest> list = new List<ClassTest>();
            Assert.False(list.HasElements());

            for (int i = 0; i < 500; i++)
            {
                list.Add(new ClassTest
                {
                    ID = i + 1,
                    NAME = ((char)(i % 2 + 65)) + (i + 1).ToString()
                });
            }

            Assert.True(list.HasElements());
        }

        [Test]
        public void TruncateTest()
        {
            double originalValue = 10.33333333333;
            var truncated = (originalValue).Truncate(2);

            Assert.AreNotEqual(originalValue, truncated);
        }

        [Test]
        public void RemoveChars() 
        {
            string original = "çãopáulô";
            string removed = original.RemoveSpecialChars();
            Assert.AreNotEqual(original, removed);
        }

        [Test]
        public void ReplaceChars()
        {
            string original = "çãopáulô";
            string removed = original.ReplaceSpecialChars();
            Assert.AreNotEqual(original, removed);
        }

        [Test]
        public void IsImage()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Path.GetDirectoryName(
                Uri.UnescapeDataString(uri.Path)
            );

            var file = File.OpenRead(Path.Combine(path, @"Assets\Images\IsImage.jpg"));
            Assert.IsNotNull(file);

            var isImage = file.IsImage();
            Assert.IsTrue(isImage);

            file = File.OpenRead(Path.Combine(path, @"Assets\Files\TestFile.txt"));
            Assert.IsNotNull(file);

            isImage = file.IsImage();
            Assert.IsFalse(isImage);
        }

        [Test]
        public void TestDictionaryToQueryStringExtension()
        {
            var dict = new Dictionary<string, object>();
            var testClass = new ClassTest();

            testClass.ID = 1;
            testClass.NAME = "name";

            dict.Add("id", testClass.ID);
            dict.Add("name", testClass.NAME);

            var queryString = dict.ToQueryString();

            Assert.AreEqual("id=1&name=name", queryString);
        }

        [Test]
        public void TestDictionaryStringStringToQueryStringExtension()
        {
            var dict = new Dictionary<string, string>();
            var testClass = new ClassTest();

            testClass.ID = 1;
            testClass.NAME = "name";
            dict.Add("name", testClass.NAME);

            var queryString = dict.ToQueryString();

            Assert.AreEqual("name=name", queryString);
        }

        [Test]
        public void TestEmptyDictionaryToQueryStringExtension()
        {
            var dict = new Dictionary<string, object>();

            var queryString = dict.ToQueryString();

            Assert.AreEqual("", queryString);
        }

        [Test]
        public void TestStringStripTags()
        {
            Assert.AreEqual("<p>lorem <b>ipsum</b> dolor <a href='teste'>sit</a></p>".StripHtmlTags(), "lorem ipsum dolor sit");
        }

        [Test]
        public void TestStringStripTagAttrs()
        {
            Assert.AreEqual("<p >lorem <a href=\"teste\">ipsum</a></p>", "<p style='font-face:verdana'>lorem <a href=\"teste\">ipsum</a></p>".StripHtmlAttrs("href"));
            Assert.AreEqual("<p style='font-face:verdana'>lorem <a >ipsum</a></p>", "<p style='font-face:verdana'>lorem <a href=\"teste\">ipsum</a></p>".StripHtmlAttrs("style"));
            Assert.AreEqual("<p style='font-face:verdana'>lorem <a href=\"teste\">ipsum</a></p>", "<p style='font-face:verdana'>lorem <a href=\"teste\">ipsum</a></p>".StripHtmlAttrs(""));
        }

        class ClassTest
        {
            public int ID { get; set; }
            public string NAME { get; set; }
        }
    }
}
