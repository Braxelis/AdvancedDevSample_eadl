using System;
using System.Collections.Generic;
using System.Linq;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Entities;
using AdvancedDevSample.Domain.Interfaces;

namespace AdvancedDevSample.Application.Services
{
    /// <summary>
    /// Service applicatif pour gérer les fournisseurs.
    /// </summary>
    public class SupplierService
    {
        private readonly ISupplierRepository _repo;

        public SupplierService(ISupplierRepository repo)
        {
            _repo = repo;
        }

        // --------- Cas d'usage : Lister les fournisseurs ---------
        public IReadOnlyList<SupplierDto> GetAll()
        {
            var suppliers = _repo.ListAll();
            return suppliers
                .Select(MapToDto)
                .ToList();
        }

        // --------- Cas d'usage : Détail fournisseur ---------
        public SupplierDto GetById(Guid id)
        {
            var supplier = GetSupplier(id);
            return MapToDto(supplier);
        }

        // --------- Cas d'usage : Créer un fournisseur ---------
        public SupplierDto Create(CreateSupplierRequest request)
        {
            var supplier = new Supplier(request.Name, request.Email, request.Phone, request.Address);
            _repo.Add(supplier);
            return MapToDto(supplier);
        }

        // --------- Cas d'usage : Mettre à jour un fournisseur ---------
        public void Update(Guid id, UpdateSupplierRequest request)
        {
            var supplier = GetSupplier(id);
            supplier.UpdateContactInfo(request.Name, request.Email, request.Phone, request.Address);
            _repo.Save(supplier);
        }

        // --------- Cas d'usage : Supprimer un fournisseur ---------
        public void Delete(Guid id)
        {
            var supplier = GetSupplier(id);
            _repo.Remove(id);
        }

        // --------- Cas d'usage : Activer / Désactiver ---------
        public void Activate(Guid id)
        {
            var supplier = GetSupplier(id);
            supplier.Activate();
            _repo.Save(supplier);
        }

        public void Deactivate(Guid id)
        {
            var supplier = GetSupplier(id);
            supplier.Deactivate();
            _repo.Save(supplier);
        }

        // --------- Méthodes privées utilitaires ---------
        private Supplier GetSupplier(Guid id)
        {
            return _repo.GetById(id)
                   ?? throw new ApplicationServiceException("Fournisseur non trouvé.");
        }

        private static SupplierDto MapToDto(Supplier supplier) =>
            new SupplierDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Email = supplier.Email,
                Phone = supplier.Phone,
                Address = supplier.Address,
                IsActive = supplier.IsActive
            };
    }
}
