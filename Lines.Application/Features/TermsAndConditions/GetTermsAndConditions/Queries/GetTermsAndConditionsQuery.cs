using Lines.Application.Common;
using Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.DTOs;
using Lines.Domain.Enums;
using Lines.Domain.Models.TermsAndConditions;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lines.Application.Features.TermsAndConditions.GetTermsAndConditions.Queries
{
    public record GetTermsAndConditionsQuery () : IRequest<List<GetTermsAndConditionsDTO>>;
    public class GetTermsAndConditionsHandler(
      RequestHandlerBaseParameters parameters,
      IRepository<Domain.Models.TermsAndConditions.TermsAndConditions> termsRepository)
      : RequestHandlerBase<GetTermsAndConditionsQuery, List<GetTermsAndConditionsDTO>>(parameters)
    {   
        public override async Task<List<GetTermsAndConditionsDTO>> Handle(GetTermsAndConditionsQuery request, CancellationToken cancellationToken)
        {
            var termsAndConditions =  termsRepository
                .Get()
                .OrderBy(x => x.TermsType);

            var result = termsAndConditions
                .GroupBy(x => x.TermsType)
                .Select(group => new GetTermsAndConditionsDTO
                {
                    TermsType = group.Key,
                    Details = group
                        .OrderBy(x => x.Order)
                        .Select(x => new TermsAndConditionsDataDTO
                        {
                            Id = x.Id,
                            Header = x.Header,
                            Content = x.Content,
                            Order = x.Order,
                            UpdatedAt = x.UpdatedDate
                        })
                        .ToList()
                })
                .ToList();

            return result;
        }
    }
}
