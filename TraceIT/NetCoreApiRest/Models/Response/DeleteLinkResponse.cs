﻿using System;
using WSTraceIT.Models.Base;

namespace WSTraceIT.Models.Response
{
	public class DeleteLinkResponse: TraceITResponse
	{
		public DeleteLinkResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}
}