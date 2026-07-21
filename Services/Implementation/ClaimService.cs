using Final_Insure.DTOs;
using Final_Insure.Models;
using Final_Insure.Repositories.Interfaces;
using Final_Insure.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Final_Insure.Services.Implementations
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepo;
        private readonly IWebHostEnvironment _env;
        private readonly ISettlementLogRepository _settlementRepo;

        // We inject the repositories we need to do our work
        public ClaimService(IClaimRepository claimRepo, ISettlementLogRepository settlementRepo, IWebHostEnvironment env)
        {
            _claimRepo = claimRepo;
            _settlementRepo = settlementRepo;
            _env = env;
        }

        public async Task<Claim> RegisterClaimAsync(CreateClaimDTO dto, int customerId)
        {
            // 1. Map the secure DTO to the real database model
            var claim = new Claim
            {
                PolicyId = dto.PolicyId,
                CustomerId = customerId,
                ClaimType = dto.ClaimType,
                ClaimAmount = dto.ClaimAmount,
                IncidentDate = dto.IncidentDate,
                Description = dto.Description,
                AccidentAddress = dto.AccidentAddress, // <-- Now capturing the address!

                // 2. Apply business rules
                ClaimStatus = ClaimStatus.Registered,
                SubmittedAt = DateTime.UtcNow,
                ClaimDocuments = new List<ClaimDocument>() // Initialize the list to hold files
            };

            // 3. Process and Save the Uploaded Files
            if (dto.DamagePhoto != null)
            {
                claim.ClaimDocuments.Add(new ClaimDocument
                {
                    DocumentName = "Damage Photo",
                    FilePath = await SaveFileAsync(dto.DamagePhoto),
                    VerificationStatus = VerificationStatus.Pending
                });
            }

            if (dto.FIRDocument != null)
            {
                claim.ClaimDocuments.Add(new ClaimDocument
                {
                    DocumentName = "FIR Copy",
                    FilePath = await SaveFileAsync(dto.FIRDocument),
                    VerificationStatus = VerificationStatus.Pending
                });
            }

            if (dto.GarageReceipt != null)
            {
                claim.ClaimDocuments.Add(new ClaimDocument
                {
                    DocumentName = "Garage Receipt",
                    FilePath = await SaveFileAsync(dto.GarageReceipt),
                    VerificationStatus = VerificationStatus.Pending
                });
            }

            // 4. Save everything to the database
            // (Entity Framework is smart enough to save the Claim AND all the ClaimDocuments at once!)
            await _claimRepo.AddAsync(claim);

            return claim;
        }

        // --- NEW HELPER METHOD TO SAVE PHYSICAL FILES TO THE SERVER ---
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return string.Empty;

            // 1. Point to wwwroot/uploads/claims
            string uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "claims");

            // 2. Create the folder if it doesn't exist yet
            Directory.CreateDirectory(uploadsFolder);

            // 3. Create a unique filename so customers don't overwrite each other's photos
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            // 4. Copy the file to the server's hard drive
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 5. Return the web-friendly path to save in the database
            return $"/uploads/claims/{uniqueFileName}";
        }





        public async Task MoveToSurveyorAsync(AssignSurveyorDTO dto)
        {
            var claim = await _claimRepo.GetByIdAsync(dto.ClaimId);
            if (claim != null)
            {
                // Update the status and move it to the Surveyor's desk
                claim.ClaimStatus = ClaimStatus.UnderAssessment;
                await _claimRepo.UpdateAsync(claim);
            }
        }

        public async Task ProcessFinalSettlementAsync(ProcessSettlementDTO dto)
        {
            var claim = await _claimRepo.GetByIdAsync(dto.ClaimId);
            if (claim != null)
            {
                // 1. Update the claim status
                claim.ClaimStatus = dto.FinalStatus;
                await _claimRepo.UpdateAsync(claim);

                // 2. Create the permanent settlement log record
                var settlement = new SettlementLog
                {
                    ClaimId = dto.ClaimId,
                    SettlementAmount = dto.SettlementAmount,
                    SettlementStatus = dto.FinalStatus.ToString(),
                    Remarks = dto.Remarks,
                    ProcessedAt = DateTime.UtcNow
                };
                await _settlementRepo.AddAsync(settlement);
            }
        }
    }
}