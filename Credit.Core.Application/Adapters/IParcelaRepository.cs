﻿using Credit.Core.Domain.Entities;

namespace Credit.Core.Application.Adapters
{
    public interface IParcelaRepository
    {
        Task<bool> Pagar(Parcela parcela);

        Task<Parcela?> FindByUid(Guid uid);
    }
}
