﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using OtripleS.Web.Api.Models.CourseAttachments;
using OtripleS.Web.Api.Models.CourseAttachments.Exceptions;

namespace OtripleS.Web.Api.Services.CourseAttachments
{
    public partial class CourseAttachmentService
    {
        private delegate ValueTask<CourseAttachment> ReturningCourseAttachmentFunction();

        private async ValueTask<CourseAttachment> TryCatch(
            ReturningCourseAttachmentFunction returningCourseAttachmentFunction)
        {
            try
            {
                return await returningCourseAttachmentFunction();
            }
            catch (NullCourseAttachmentException nullCourseAttachmentException)
            {
                throw CreateAndLogValidationException(nullCourseAttachmentException);
            }
            catch (InvalidCourseAttachmentException invalidCourseAttachmentException)
            {
                throw CreateAndLogValidationException(invalidCourseAttachmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsCalendarEntryAttachmentException =
                    new AlreadyExistsCourseAttachmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCalendarEntryAttachmentException);
            }
        }


        private CourseAttachmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var courseAttachmentValidationException = new CourseAttachmentValidationException(exception);
            this.loggingBroker.LogError(courseAttachmentValidationException);

            return courseAttachmentValidationException;
        }

    }
}
