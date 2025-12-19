using Lines.Application.Features.Trips.GetTripInvoice.DTOs;
using Lines.Application.Features.Trips.GetTripInvoice.Orchestrators;
using Lines.Presentation.Common;
using Microsoft.AspNetCore.Mvc;

namespace Lines.Presentation.Endpoints.Trips.GetInvoice;

public class GetTripInvoiceEndpoint : BaseController<GetTripInvoiceRequest, GetTripInvoiceResponse>
{
    private readonly BaseControllerParams<GetTripInvoiceRequest> _dependencyCollection;
    
    public GetTripInvoiceEndpoint(BaseControllerParams<GetTripInvoiceRequest> dependencyCollection) 
        : base(dependencyCollection)
    {
        _dependencyCollection = dependencyCollection;
    }

    [HttpGet("trips/{tripId}/invoice")]
    public async Task<IActionResult> GetInvoice(Guid tripId, [FromQuery] string format = "json")
    {
        var request = new GetTripInvoiceRequest(tripId);
        
        var validateResult = await ValidateRequestAsync(request);
        if (!validateResult.IsSuccess)
        {
            return BadRequest(validateResult);
        }

        var result = await _mediator.Send(new GetTripInvoiceOrchestrator(tripId));

        if (!result.IsSuccess)
        {
            return BadRequest(HandleResult<TripInvoiceDto, GetTripInvoiceResponse>(result));
        }

        if (format.ToLower() == "json")
        {
            var response = HandleResult<TripInvoiceDto, GetTripInvoiceResponse>(result);
            return Ok(response);
        }
        else
        {
            // For now, return JSON format as requested
            // PDF generation can be added later when QuestPDF is implemented
            var response = HandleResult<TripInvoiceDto, GetTripInvoiceResponse>(result);
            return Ok(response);
        }
    }
}
