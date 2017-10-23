using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Challenge
{
	[TestFixture]
	public class WordsStatistics_Tests
	{
		public static string Authors = "Starcev Arapova"; // "Egorov Shagalina"

		public virtual IWordsStatistics CreateStatistics()
		{
			// меняется на разные реализации при запуске exe
			return new WordsStatistics();
		}

		private IWordsStatistics statistics;

		[SetUp]
		public void SetUp()
		{
			statistics = CreateStatistics();
		}

		[Test]
		public void GetStatistics_IsEmpty_AfterCreation()
		{
			statistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void GetStatistics_ContainsItem_AfterAddition()
		{
			statistics.AddWord("abc");
			statistics.GetStatistics().Should().Equal(Tuple.Create(1, "abc"));
		}

		[Test]
		public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
		{
			statistics.AddWord("abc");
			statistics.AddWord("def");
			statistics.GetStatistics().Should().HaveCount(2);
		}

		[Test]
		public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords_WithTwoGet()
		{
			statistics.AddWord("abc");
			statistics.AddWord("def");
			statistics.GetStatistics().Should().HaveCount(2);
			statistics.AddWord("d");
			statistics.GetStatistics().Should().HaveCount(3);
		}

		[Test]
		public void GetStatistics_ContainsItem_WithLongName()
		{
			statistics.AddWord("12345678901");
			statistics.GetStatistics().Should().Equal(Tuple.Create(1, "1234567890"));
		}

		[Test]
		public void GetStatistics_ContainsItem_WithNotLongName()
		{
			statistics.AddWord("123456");
			statistics.GetStatistics().Should().Equal(Tuple.Create(1, "123456"));
		}

		[Test]
		public void GetStatistics_ContainsItem_WithBigLowerCase()
		{
			statistics.AddWord("Abc");
			statistics.GetStatistics().Should().Equal(Tuple.Create(1, "abc"));
		}

		[Test]
		public void GetStatistics_ContainsItem_WithALotOfSpace()
		{
			statistics.AddWord("          a");
			statistics.GetStatistics().Should().HaveCount(1);
		}

		[Test]
		public void GetStatistics_ContainsRussianChar1()
		{
			statistics.AddWord("Ъ");
			statistics.GetStatistics().Should().Equal(Tuple.Create(1, "ъ"));
		}

		[Test]
		public void GetStatistics_ContainsRussianChar()
		{
			statistics.AddWord("Ё");
			statistics.GetStatistics().Should().Equal(Tuple.Create(1, "ё"));
		}

		[Test]
		public void GetStatistics_ContainsItem_WithCorrectOrder()
		{
			statistics.AddWord("Abc");
			statistics.AddWord("Aac");
			statistics.GetStatistics().Should().BeInAscendingOrder();
		}

		[Test]
		public void GetStatistics_InAscendingOrder_WithCorrectOrder_WithSimmilarString()
		{
			statistics.AddWord("Abc");
			statistics.AddWord("Abc");
			statistics.AddWord("Aac");
			statistics.GetStatistics().Should().BeInDescendingOrder();

		}

		[Test]
		public void GetStatistics_ContainsItem_WithEmptyString()
		{
			statistics.AddWord("");
			statistics.GetStatistics().Should().HaveCount(0);
		}

		[Test]
		public void GetStatistics_ContainsItem_WithWhiteSpace()
		{
			statistics.AddWord("   ");
			statistics.GetStatistics().Should().HaveCount(0);
		}

		[Test, Timeout(5000)]
		public void GetStatistics_ContainsItem_WithManyItems()
		{
			for (int i = 0; i < 200000; i++)
				statistics.AddWord("a" + i);
			statistics.GetStatistics().Should().HaveCount(200000);
		}
		[Test]
		public void GetStatistics_NullString()
		{
			Assert.Throws<ArgumentNullException>(() => statistics.AddWord(null));
		}
		// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki
	}
}