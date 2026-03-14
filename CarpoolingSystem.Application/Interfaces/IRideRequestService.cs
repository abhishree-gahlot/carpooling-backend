using CarpoolingSystem.Application.DTOs;
using CarpoolingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarpoolingSystem.Application.Interfaces {
    public interface IRideRequestService {

        Task<RideRequests> CreateRequestAsync(RideRequestCreateDto dto, Guid passengerId);
        Task<RideRequests> UpdateRequestAsync(Guid requestId, RideRequestUpdateDto dto);
        Task<RideRequests> CancelRequestAsync(Guid requestId, Guid passengerId);
        Task<IEnumerable<RideRequests>> GetAllRequestsAsync();
        Task<IEnumerable<RideRequests>> GetRequestsByPassengerIdAsync(Guid passengerId);
        Task<RideRequests?> GetRequestByIdAsync(Guid requestId);
        Task<bool> DeleteRequestAsync(Guid requestId);
        Task<IEnumerable<RideRequests>> GetPendingRequestsAsync();
    }
}
