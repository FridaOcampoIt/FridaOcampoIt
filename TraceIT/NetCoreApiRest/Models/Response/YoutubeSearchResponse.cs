using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.Base.Families;

namespace WSTraceIT.Models.Response
{
	public class YoutubeSearchResponse: TraceITResponse
	{
		public List<YoutubeData> youtubeData { get; set; }

		public YoutubeSearchResponse()
		{
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
			this.youtubeData = new List<YoutubeData>();
		}
	}
}
