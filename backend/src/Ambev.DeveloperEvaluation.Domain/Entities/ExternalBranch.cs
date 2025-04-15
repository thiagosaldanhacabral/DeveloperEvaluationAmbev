using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

    /// <summary>
    /// Represents a branch where sales are made.
    /// Includes details such as the branch's name and location.
    /// </summary>
    public class ExternalBranch : BaseEntity
    {
        /// <summary>
        /// Gets the name of the branch.
        /// </summary>
        public string BranchName { get; private set; }

        /// <summary>
        /// Gets the location of the branch.
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the item has been active.
        /// </summary>
        public bool IsActive { get; private set; }
        
        /// <summary>
        /// Inactive the customer.
        /// </summary>
        public void Inactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
    
        /// <summary>
        /// Active the customer.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalBranch"/> class.
        /// </summary>
        public ExternalBranch(string branchName, string location)
        {
            BranchName = branchName;
            Location = location;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
        
        /// <summary>
        /// Performs validation of the external branch entity using the ExternalBranchValidator rules.
        /// </summary>
        /// <returns>
        /// A <see cref="ValidationResultDetail"/> containing:
        /// - IsValid: Indicates whether all validation rules passed
        /// - Errors: Collection of validation errors if any rules failed
        /// </returns>
        /// <remarks>
        /// <listheader>The validation includes checking:</listheader>
        /// <list type="bullet">Branch name is not empty</list>
        /// <list type="bullet">Branch name length does not exceed maximum</list>
        /// <list type="bullet">Branch location is provided</list>
        /// <list type="bullet">Branch location length does not exceed maximum</list>
        /// </remarks>
        public ValidationResultDetail Validate()
        {
            var validator = new ExternalBranchValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
