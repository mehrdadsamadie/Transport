[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Transport.MVCGridConfig), "RegisterGrids")]

namespace Transport
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;
    using Models;
    using Domain.Repository;
    using Domain.Entity;
    using Microsoft.AspNet.Identity;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            
            MVCGridDefinitionTable.Add("RequestGrid", new MVCGridBuilder<RequestViewModelIndex>()
                .WithAuthorizationType(AuthorizationType.AllowAnonymous)
              //  .WithAdditionalQueryOptionNames("RequestPersonel")
              .WithSorting(sorting: true, defaultSortColumn: "Id", defaultSortDirection: SortDirection.Asc)
                .AddColumns(cols =>
                {
                    // Add your columns here
                    cols.Add().WithColumnName("Id").WithValueExpression(i => i.Id.ToString()).WithVisibility(false);
                    cols.Add().WithColumnName("Select").WithValueExpression(i => "<input name='allChk' type='checkbox' /> ").WithHtmlEncoding(false).WithHeaderText("انتخاب");

                    cols.Add().WithColumnName("PersonelName")
                        .WithHeaderText("درخواست کننده")
                        .WithValueExpression(i => i.PersonelName); // use the Value Expression to return the cell text for this column
                    cols.Add().WithColumnName("StartTime")
                    .WithHeaderText("زمان شروع")
                    .WithSorting(true)
                    .WithValueExpression(i => i.StartTime.ToString());
                    cols.Add().WithColumnName("EndTime")
                    .WithHeaderText("زمان پایان")
                    .WithSorting(true)
                    .WithValueExpression(i => i.EndTime.ToString());
                    cols.Add().WithColumnName("Source")
                    .WithHeaderText("مبدا")
                    .WithValueExpression(i => i.Biginning);
                    cols.Add().WithColumnName("Destination")
                    .WithHeaderText("مقصد")
                    .WithValueExpression(i => i.Destination);
                    cols.Add().WithColumnName("IsLock")
                    .WithHeaderText("قفل شده")
                    .WithValueExpression(i => i.IsLock.ToString())
                  //  .WithCellCssClassExpression(i => i.IsLock ? "fa fa-check text-success" : "fa fa-times text-danger")
                  ;

                    cols.Add().WithColumnName("IsAcceptTranasport")
                    .WithHeaderText("پذیرش توسط نقلیه")
                    .WithValueExpression(i => i.IsAcceptTranasport.ToString())
                    .WithCellCssClassExpression(i => (i.IsAcceptTranasport == null || i.IsAcceptTranasport == false) ?
                    "fa fa-times text-danger" : "fa fa-check text-success");

                    cols.Add().WithColumnName("IsAcceptManager")
                    .WithHeaderText("پذیرش توسط مدیر")
                    .WithValueExpression(i => i.IsAcceptManager.ToString())
               //     .WithCellCssClassExpression(i => (i.IsAcceptManager == null || i.IsAcceptManager == false) ?
                //    "fa fa-times text-danger" : "fa fa-check text-success")
                ;

                    cols.Add().WithColumnName("IsEmergancy")
                    .WithHeaderText("ضروری")
                    .WithValueExpression(i => i.IsEmergancy.ToString())
                  //  .WithCellCssClassExpression(i => (i.IsEmergancy == null || i.IsEmergancy == false) ?
                  //  "fa fa-times text-danger" : "fa fa-check text-success")
                  ;

                    cols.Add().WithColumnName("Edit")
                        .WithHeaderText("ویرایش")
                        .WithValueExpression((i, c) => c.UrlHelper.Action("Create", "Request", new { id = i.Id }))
                     //   .WithCellCssClassExpression(i => "btn btn-success fa fa-pencil-square-o text-info")
                     .WithValueTemplate("<a href='{Value}'>{Model.Id}</a>", false)
                    ;

                    cols.Add().WithColumnName("Delete")
                        .WithHeaderText("حذف")
                        .WithValueExpression((i, c) => c.UrlHelper.Action("Delete", "Request", new { id = i.Id }))
                      //  .WithCellCssClassExpression(i => "btn btn-danger fa fa-times text-danger")
                      .WithValueTemplate("<a href='{Value}'>{Model.Id}</a>", false)
                      ;

                    cols.Add().WithColumnName("Service")
                        .WithHeaderText("سرویس")
                        .WithValueExpression((i, c) => c.UrlHelper.Action(i.ServiceId == null ? "ConvertRequestToService" : "Create", "Service",
                        new {  id = i.ServiceId == null ? i.Id : i.ServiceId }))
                      //  .WithCellCssClassExpression(i => "btn btn-danger fa fa-times text-danger")
                      .WithValueTemplate("<a href='{Value}'>{Model.Id}</a>", false)
                      ;

                })
                .WithPaging(true, 10, true, 100)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var RequestRepo = DependencyResolver.Current.GetService<RequestRepository>();
                    var FactoryUserManagerRepo = DependencyResolver.Current.GetService<FactoryUserManagerRepository>();
                    var PersonelRepo = DependencyResolver.Current.GetService<PersonnelRepository>();
                    var _personel = PersonelRepo.GetByUserId(HttpContext.Current.User.Identity.GetUserId());
                    int? RequestPersonel = null; string PersonelSearchString = options.GetAdditionalQueryOptionString("RequestPersonel");
                    if(!string.IsNullOrWhiteSpace(PersonelSearchString))
                     RequestPersonel = int.Parse(PersonelSearchString);
                    var _Requests = new List<Request>(); 
                    if (HttpContext.Current.User.IsInRole("Admin"))
                    {
                        _Requests = RequestRepo.All(_personel.Id, false, null, null, RequestPersonel);
                    }
                    else if (HttpContext.Current.User.IsInRole("FactorManager"))
                    {
                        var _factoryUserManager = FactoryUserManagerRepo.GetByPersonnelId(_personel.Id);
                        _Requests = RequestRepo.All(_personel.Id, false, null, null, _personel.Id,
                            _factoryUserManager == null ? (int?)null : _factoryUserManager.FactoryUnitId)
                            .Where(x => RequestPersonel == null || x.PersonelId == RequestPersonel).ToList();
                    }
                    else
                    {
                        _Requests = RequestRepo.All(_personel.Id, false, null, null, _personel.Id).ToList();
                    }
                    var _requestView = new List<RequestViewModelIndex>();
                    if (_Requests.Count > 0)
                    {
                        _requestView = _Requests.Select(x => new RequestViewModelIndex
                        {
                            StartTime = x.StartTime,
                            EndTime = x.EndTime,
                            IsLock = x.IsLock,
                            IsAcceptTranasport = x.IsAcceptTranasport,
                            IsAcceptManager = x.IsAcceptManager,
                            Biginning = x.BeginingAddress.Address1,
                            Destination = x.DestinationAddress.Address1,
                            IsEmergancy = x.IsEmergancy,
                            PersonelName = x.Personnel.FullName,
                            Id = x.Id,
                            ServiceId = x.Services.FirstOrDefault(y => y.RequestId == x.Id) == null ? (int?)null : x.Services.FirstOrDefault(y => y.RequestId == x.Id).Id
                        }).OrderBy(x => x.Id).ToList();
                    }
                    return new QueryResult<RequestViewModelIndex>()
                    {
                        Items = _requestView.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value),
                        TotalRecords = _requestView.Count // if paging is enabled, return the total number of records of all pages
                    };

                })

            );
            
        }
    }
}