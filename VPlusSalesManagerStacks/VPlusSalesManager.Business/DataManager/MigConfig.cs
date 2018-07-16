﻿using System;
using System.IO;
using VPlusSalesManager.Business.DataManager;
using XPLUG.WEBTOOLS;
// ReSharper disable once CheckNamespace
namespace VPlusSalesManager.Business.Migrations
{
    internal partial class Configuration
    {

        private void ProcessSeed(VPlusSalesManagerModel context)
        {
            try
            {
                var basePath = getBasePath();
                if (string.IsNullOrEmpty(basePath)) { return; }
                //if (!context.Banks.Any() && !context.LocalAreas.Any() && !context.TechnicalSettings.Any())
                //{
                //    var query = GetFromResources(basePath + "\\SqlFiles\\default_lookups.sql");
                //    if (!string.IsNullOrEmpty(query))
                //    {
                //        context.Database.ExecuteSqlCommand(query);
                //    }
                //}
                //if (!context.BetSchedules.Any() && !context.Lotteries.Any() && !context.BetTypes.Any())
                //{
                //    var query = GetFromResources(basePath + "\\SqlFiles\\lottoMat_setup.sql");
                //    if (!string.IsNullOrEmpty(query))
                //    {
                //        context.Database.ExecuteSqlCommand(query);
                //    }
                //}
                //if (!context.PinDenominations.Any() && !context.PinTypes.Any())
                //{
                //    var query = GetFromResources(basePath + "\\SqlFiles\\scratch_card.sql");
                //    if (!string.IsNullOrEmpty(query))
                //    {
                //        context.Database.ExecuteSqlCommand(query);
                //    }
                //}
            }
            catch (Exception ex)
            {
                ErrorManager.LogApplicationError(ex.StackTrace, ex.Source, ex.Message);
            }

        }

        private string getBasePath()
        {
            var currentDomain = AppDomain.CurrentDomain;

            string dirPath;
            if (currentDomain.BaseDirectory.Contains("\\bin\\Debug"))
            {
                dirPath = currentDomain.BaseDirectory.Replace("\\bin\\Debug\\", "");
            }
            else if (currentDomain.BaseDirectory.Contains("\\bin\\Release"))
            {
                dirPath = currentDomain.BaseDirectory.Replace("\\bin\\Release\\", "");
            }
            else
            {
                dirPath = currentDomain.BaseDirectory;
            }
            return dirPath;
        }

        internal string GetFromResources(string resourceName)
        {
            try
            {
                using (var reader = new StreamReader(resourceName))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
