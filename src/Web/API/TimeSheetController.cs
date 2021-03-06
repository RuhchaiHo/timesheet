﻿namespace Web.API
{
    using Common;
    using Extension;
    using Helper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Model;
    using Platform.Util;
    using Service.User;
    using System;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    public class TimeSheetController : BaseController
    {
        private static ILogger _logger = LoggerUtil.CreateLogger<TimeSheetController>();

        [Route("GetTimeSheetModels")]
        [HttpPost]
        public List<TimeSheetOverviewModel> GetTimeSheetModels()
        {
            try
            {
                var helper = new TimeSheetHelper(this);
                return helper.BuildTimeSheetOverViewModel(this.GetUserId());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [Route("ReturnTimeSheet")]
        [HttpPost]
        public JsonResult ReturnTimeSheet(string userId, string monday)
        {
            try
            {
                var departmentService = this.GetService<DepartmentService>();

                //没有权限
                if (!departmentService.IsBoss(this.GetUserId(), userId))
                {
                    return Json(new { errorMsg = "You don't have right!" });
                }

                var userTimeSheetStatusService = this.GetService<UserTimeSheetStatusService>();
                userTimeSheetStatusService.ReturnTimeSheet(monday, userId);
                return Json(new { successMsg = string.Format("Return timesheet({0}) successfully!", monday + "_" + userId) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(new { errorMsg = ex.Message });
            }
        }
    }
}
