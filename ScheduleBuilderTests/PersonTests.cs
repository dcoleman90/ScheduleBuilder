﻿using ScheduleBuilder.Model;
using System;
using Xunit;

namespace ScheduleBuilderTests
{
    //This testing framework was develeped using methods from Tim Corey Course found
    //https://www.youtube.com/watch?v=ub3P8c87cwk
    public class PersonTests
    {
        public Person person = new Person
        {
            LastName = "Coleman",
            FirstName = "Drew",
            Ssn = "123456789",
            Gender = "Male",
            DateOfBirth = DateTime.Now,
            RoleId = 1,
            StatusId = 1,
            Email = "Drew@email.com",
            StreetAddress = "1149 Grove",
            Zipcode = "30145",
            Phone = "4543631011",
            Password = "pass",
            Username = "test"
        };

        [Fact]
        public void TestShouldReturnPersonFirstAndLastName()
        {
            //Arange - set up the test - arrange values
            string expected = "Drew Coleman";
            //Act - do the action which is being tested
            string actual = this.person.GetFullName();
            //Assert - test expected value
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestShouldReturnFirstName()
        {
            string expected = "Drew";

            //Act - do the action which is being tested
            string actual = this.person.FirstName;


            //Assert - test expected value
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestShouldReturnLasttName()
        {
            string expected = "Coleman";

            //Act - do the action which is being tested
            string actual = this.person.LastName;


            //Assert - test expected value
            Assert.Equal(expected, actual);
        }
    }
}