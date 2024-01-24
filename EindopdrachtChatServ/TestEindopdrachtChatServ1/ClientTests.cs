using Client;
using NUnit.Framework;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestEindopdrachtChatServ1;


public class ClientTests
{
    HelpClass helpClass;

    [SetUp]
    public void SetUp()
    {
        helpClass = new HelpClass();
    }

    [Test]
    public void TestEmptyName()
    {
        //Arrange
        string emptyName = "     ";

        //Act
        bool notEmpty = helpClass.ReturnTrueIfNotEmpty(emptyName);

        //Assert
        Assert.IsFalse(notEmpty);
    }

    [Test]
    public void TestValidName()
    {
        //Arrange
        string validName = "Sjors";

        //Act
        bool notEmpty = helpClass.ReturnTrueIfNotEmpty(validName);

        //Assert
        Assert.IsTrue(notEmpty);
    }

}