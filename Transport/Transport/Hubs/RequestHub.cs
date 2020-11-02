using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Domain.Entity;
using Domain.Repository;

namespace Transport.Hubs
{
    public class RequestHub : Hub
    {
        RequestRepository RequestRepository = new RequestRepository();
        public void Send()
        {

            Clients.All.addNewMessageToPage(null);
        }
        public RequestHubView addNewMessageToPage(int? requestId)
        {
            if (requestId != null)
            {
                var _request = RequestRepository.GetById(requestId.Value);
                var _requesthub = new RequestHubView()
                {
                    FullName = _request.Personnel.FullName,
                    StartTime = _request.StartTime == null ? null : _request.StartTime.Value.ToString(@"hh\:mm"),
                    IsLocal = _request.IsLocal,

                };
                return _requesthub;
            }
            else { return null; }

        }
    }

    public class RequestHubView
    {
        public string FullName { get;  set; }
        public string FactoryUnit { get;  set; }
        public bool IsLocal { get;  set; }
        public string StartTime { get;  set; }
    }
}