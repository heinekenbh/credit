﻿using Credit.Core.Domain.Exceptions.Cpf;
using Credit.Core.Domain.ValueObjects;

namespace Credit.Core.Domain.UnitTest
{
    public class CpfTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void ShouldOccurCpfEmptyError(string valor)
        {
            var ex = Assert.Throws<CpfCoreDomainException>(() =>
            {
                Cpf cpf = valor;
            });


            Assert.Equal(CpfError.CpfEmpty.Key, ex.Errors.First().Key);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("abc.def.ghi-jk")]
        [InlineData("ABCDEFGHIJK")]
        public void ShouldOccurCpfInvalidFormatError(string valor)
        {
            var ex = Assert.Throws<CpfCoreDomainException>(() =>
            {
                Cpf cpf = valor;
            });


            Assert.Equal(CpfError.CpfInvalidFormat(valor).Key, ex.Errors.First().Key);
        }

        [Theory]
        [InlineData("9486758301")]
        [InlineData("948675830135")]
        [InlineData("948.675.830-1")]
        [InlineData("948.675.830-135")]
        [InlineData("948.675.830-13")]
        public void ShouldOccurCpfInvalidErrorString(string valor)
        {
            var ex = Assert.Throws<CpfCoreDomainException>(() =>
            {
                Cpf cpf = valor;
            });

            Assert.Equal(CpfError.CpfInvalid(valor).Key, ex.Errors.First().Key);
        }

        [Theory]
        [InlineData(9486758301)]
        [InlineData(948675830135)]
        [InlineData(94867583013)]
        public void ShouldOccurCpfInvalidErrorLong(long valor)
        {
            var ex = Assert.Throws<CpfCoreDomainException>(() =>
            {
                Cpf cpf = valor;
            });

            Assert.Equal(CpfError.CpfInvalid(valor.ToString()).Key, ex.Errors.First().Key);
        }

        [Theory]
        [InlineData("948.675.830-14")]
        [InlineData("94867583014")]
        public void ShouldReturnCpfWithoutMaskString(string valor)
        {
            Cpf cpf = valor;

            Assert.Equal("94867583014", cpf.ToString());
        }

        [Theory]
        [InlineData(94867583014)]
        public void ShouldReturnCpfWithoutMaskLong(long valor)
        {
            Cpf cpf = valor;

            Assert.Equal("94867583014", cpf.ToString());
        }

        [Theory]
        [InlineData("948.675.830-14")]
        [InlineData("94867583014")]
        public void ShouldReturnCpfWithMaskString(string valor)
        {
            Cpf cpf = valor;

            Assert.Equal("948.675.830-14", cpf.ToMaskedString());
        }

        [Theory]
        [InlineData(94867583014)]
        public void ShouldReturnCpfWithMaskLong(long valor)
        {
            Cpf cpf = valor;

            Assert.Equal("948.675.830-14", cpf.ToMaskedString());
        }
    }
}
