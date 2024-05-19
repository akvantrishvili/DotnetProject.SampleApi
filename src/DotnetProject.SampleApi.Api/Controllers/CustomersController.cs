

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using DotnetProject.SampleApi.Application.Common;
using DotnetProject.SampleApi.Application.Customers.Queries;
using DotnetProject.SampleApi.Domain.Customers;
using DotnetProject.SampleApi.Domain.Customers.Commands;
using DotnetProject.SampleApi.Domain.Customers.Commands.IdentityDocuments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace DotnetProject.SampleApi.Api.Controllers
{
    [Route("v{version:apiVersion}/customers", Name = "Customers")]
    public class CustomersController(IMediator mediator) : ApiControllerBase
    {
        /// <summary>
        /// Create customer
        /// </summary>
        /// <param name="command">Create customer command</param>
        /// <param name="cancellationToken"></param>
        /// <response code="201" example="1">Customer identifier</response>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        //[ProducesErrorCode("ObjectAlreadyExists", StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<long>> CreateCustomer([Required, FromBody] CreateCustomerCommand command,
            CancellationToken cancellationToken)
        {
            var customerId = await mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetCustomer),
                new { customerId },
                customerId);
        }

        /// <summary>
        /// Add identity document to customer
        /// </summary>
        /// <param name="customerId" example="1">Customer identifier</param>
        /// <param name="command">Add identity document command</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("{customerId}/identity-document")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> AddIdentityDocument([Required, FromRoute] long customerId,
            [Required, FromBody] AddIdentityDocumentCommand command, CancellationToken cancellationToken)
        {
            command.CustomerId = customerId;
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Delete customer identity document
        /// </summary>
        /// <param name="customerId" example="1">Customer identifier</param>
        /// <param name="documentId" example="1">Identity document identifier</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{customerId}/identity-document/{documentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteIdentityDocument([Required, FromRoute] long customerId,
            [Required, FromRoute] long documentId, CancellationToken cancellationToken)
        {
            await mediator.Send(new DeleteIdentityDocumentCommand
            {
                CustomerId = customerId,
                IdentityDocumentId = documentId
            }, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Change customer basic info, e.g. name, date of birth, ...
        /// </summary>
        /// <param name="customerId" example="1">Customer identifier</param>
        /// <param name="command">Change basic info command</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{customerId}/change-basic-info")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangeBasicInfo([Required, FromRoute] long customerId,
            [Required, FromBody] ChangeBasicInfoCommand command,
            CancellationToken cancellationToken)
        {
            command.CustomerId = customerId;
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Change customer address
        /// </summary>
        /// <param name="customerId" example="1">Customer identifier</param>
        /// <param name="command">Change address command</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{customerId}/change-address")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> ChangeAddress([Required, FromRoute] long customerId,
            [Required, FromBody] ChangeAddressCommand command,
            CancellationToken cancellationToken)
        {
            command.CustomerId = customerId;
            await mediator.Send(command, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Close customer
        /// </summary>
        /// <param name="customerId" example="1">Customer identifier</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{customerId}/close-customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> CloseCustomer([Required, FromRoute] long customerId,
            CancellationToken cancellationToken)
        {
            await mediator.Send(new CloseCustomerCommand
            {
                CustomerId = customerId
            }, cancellationToken);
            return Ok();
        }

        /// <summary>
        /// Get customer by identifier
        /// </summary>
        /// <param name="customerId" example="1">Customer identifier</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">Customer</response>
        [HttpGet("{customerId}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        public async Task<ActionResult<Customer>> GetCustomer([Required, FromRoute] long customerId,
            CancellationToken cancellationToken)
        {
            return Ok(await mediator.Send(new GetCustomerByIdQuery
            {
                Id = customerId
            }, cancellationToken));
        }

        /// <summary>
        /// Get list of customers
        /// </summary>
        /// <param name="pageIndex" example="0">Page index starting from 0</param>
        /// <param name="pageSize" example="10">Number of records per page</param>
        /// <param name="firstName" example="John">Customer first name</param>
        /// <param name="lastName" example="Doe">Customer last name</param>
        /// <param name="idNumber" example="01020304050">Customer identity number</param>
        /// <param name="customerStatus" example="1">Customer status</param>
        /// <param name="cancellationToken"></param>
        /// <response code="200">List of customers</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedList<Customer>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedList<Customer>>> GetCustomers(
            [Required, FromQuery, DefaultValue(0)] int pageIndex = 0, [Required, FromQuery] int pageSize = 10,
            [FromQuery] string? firstName = null,
            [FromQuery] string? lastName = null,
            [FromQuery] string? idNumber = null,
            [FromQuery] CustomerStatus? customerStatus = null,
            CancellationToken cancellationToken = default)
        {
            var result = await mediator.Send(new ListCustomerQuery
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                FirstName = firstName,
                LastName = lastName,
                Status = customerStatus,
                IdNumber = idNumber
            }, cancellationToken);
            return Ok(result);
        }
    }
}
