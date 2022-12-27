using System;
using System.Collections.Generic;
using WSTraceIT.Models.Base;
using WSTraceIT.Models.ModelsSQL;

namespace WSTraceIT.Models.Response
{
	public class SearchTrackingResponse : TraceITResponse
	{
		public List<TrackingDataSQL> trackingList { get; set; }

		public SearchTrackingResponse()
		{
			this.trackingList = new List<TrackingDataSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class GetTrackingInfoResponse : TraceITResponse
	{
		public List<TrackingEventInfoSQL> eventInfo { get; set; }
		public List<TrackingEventSenderInfoSQL> eventSenderInfo { get; set; }
		public List<TrackingEventLegalInfoSQL> eventLegalInfo { get; set; }
		public List<TrackingEventLegalDocsInfoSQL> eventLegalDocsInfo { get; set; }
		public List<TrackingEventRecipientInfoSQL> eventRecipientInfo { get; set; }
		public List<TrackingEventProductsInfoSQL> eventProductsInfo { get; set; }

		public List<TrackingEventCarriersInfoSQL> eventTransportInfo { get; set; }
		public List<TrackingEventProuctDetailInfoSQL> eventProdDetailInfo { get; set; }
		public List<TrackingEventTotalProdInfoSQL> eventTotalProdInfo { get; set; }
		public List<TrackingEventTotalPalletInfoSQL> eventTotalPalletInfo { get; set; }
		public List<TrackingEventTotalBoxInfoSQL> eventTotalBoxInfo { get; set; }
		public List<TrackingEventTotalQuantityInfoSQL> eventTotalQuantityInfo { get; set; }
		public List<TrackingEventTotalWeightInfoSQL> eventTotalWeightInfo { get; set; }
		public List<TrackingEventDateMinInfoSQL> eventDateMinInfo { get; set; }

		public GetTrackingInfoResponse()
		{
			this.eventInfo = new List<TrackingEventInfoSQL>();
			this.eventSenderInfo = new List<TrackingEventSenderInfoSQL>();
			this.eventLegalInfo = new List<TrackingEventLegalInfoSQL>();
			this.eventLegalDocsInfo = new List<TrackingEventLegalDocsInfoSQL>();
			this.eventRecipientInfo = new List<TrackingEventRecipientInfoSQL>();
			this.eventProductsInfo = new List<TrackingEventProductsInfoSQL>();

			this.eventTransportInfo = new List<TrackingEventCarriersInfoSQL>();
			this.eventProdDetailInfo = new List<TrackingEventProuctDetailInfoSQL>();
			this.eventTotalProdInfo = new List<TrackingEventTotalProdInfoSQL>();
			this.eventTotalPalletInfo = new List<TrackingEventTotalPalletInfoSQL>();
			this.eventTotalBoxInfo = new List<TrackingEventTotalBoxInfoSQL>();
			this.eventTotalQuantityInfo = new List<TrackingEventTotalQuantityInfoSQL>();
			this.eventTotalWeightInfo = new List<TrackingEventTotalWeightInfoSQL>();
			this.eventDateMinInfo = new List<TrackingEventDateMinInfoSQL>();
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
		}
	}

	public class SearchMovementResponse : TraceITResponse
	{
		public int moveId { get; set; }

		public SearchMovementResponse()
		{
			this.moveId = 0;
			this.messageEng = String.Empty;
			this.messageEsp = String.Empty;
        }
	}

	public class LogTrackingResponse : TraceITResponse
	{
		public LogTrackingResponse()
		{
			this.messageEsp = String.Empty;
			this.messageEng = String.Empty;
        }
    }

	public class searchDocsResponse : TraceITResponse
	{
		public List<TrackingDocumentosFamilia> docsFamilia { get; set; }
		public List<documentoStock> docsStock { get; set; }
		public List<alertas> alertas { get; set; }
		public InfoFamilia infoFamilia { get; set; }

		public searchDocsResponse()
		{
			this.docsFamilia = new List<TrackingDocumentosFamilia>();
			this.docsStock = new List<documentoStock>();
			this.alertas = new List<alertas>();
			this.infoFamilia = new InfoFamilia();
        }
    }
}
