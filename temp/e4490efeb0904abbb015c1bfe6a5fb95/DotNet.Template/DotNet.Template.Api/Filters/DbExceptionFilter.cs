using System;
using System.Data;
using System.Net;
using Alternatives.CustomExceptions;
using DotNet.Template.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Template.Api.Filters
{
    public class DbExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            Exception ex = context.Exception;

            if (ex is DBConcurrencyException)
            {
                throw new CustomApiException(StringResources.ConcurrencyExceptionOccur, HttpStatusCode.Conflict, ex);
            }

            if (ex is DbUpdateException dbUpdateException)
            {
                Exception innerEx = dbUpdateException.InnerException;

                if (innerEx == null)
                {
                    throw new CustomApiException(StringResources.DbUpdateExceptionOccur, HttpStatusCode.B21Request, ex);
                }


                if (innerEx is SqliteException sqliteException)
                {
                    if (sqliteException.ErrorCode == (int)SqlExceptionCodes.UniqueConstraintError)
                    {
                        throw new CustomApiException(StringResources.UniqueConstraintExceptionOccur, HttpStatusCode.B21Request);
                    }
                }

                if (innerEx is System.Data.SqlClient.SqlException sqlException)
                {
                    if (sqlException.ErrorCode == (int)SqlExceptionCodes.UniqueConstraintError)
                    {
                        throw new CustomApiException(StringResources.UniqueConstraintExceptionOccur, HttpStatusCode.B21Request);
                    }
                }

                throw new CustomApiException(StringResources.DbUpdateExceptionOccur, HttpStatusCode.B21Request, ex);
            }


            throw ex;
        }

        private enum SqlExceptionCodes
        {
            UniqueConstraintError = -2147467259
        }
    }
}