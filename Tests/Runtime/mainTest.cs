using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Android {

    public class mainTest {


        [SetUp]
        public void Setup(){
            
        }

        [Test]
        public void testHelloWorld(){

            
            string expected = "helloworld";
            string result = "helloworld";

            Assert.AreEqual( expected, result );
        }
        
    }

}
