using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.ClientModels.Base.Helpers
{
	public static class FluentValidationHelper
	{
		public static List<string> GetErrorMessage(List<ValidationFailure> errors)
		{
			List<string> errorsMessages = new List<string>();
			foreach (var failure in errors)
			{
				errorsMessages.Add(failure.ErrorMessage);
			}
			return errorsMessages;
		}
	}
}
