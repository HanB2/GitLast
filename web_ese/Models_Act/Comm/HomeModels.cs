using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_ese.Models_Act.Comm
{
	public class HomeModels : SeachModel
	{

		public List<ETM_LST> ETM_NOTICE { get; set; }  //ETOMARS 공지사항 
		public List<EST_LST> EST_NOTICE { get; set; }  //STATION 공지사항
		
		public int GOODS_CNT { get; set; }		//등록 상품 갯수
		public int GODS_STOCK { get; set; }		//보관상품 총 갯수
		public int CS_CNT { get; set; }         //미답변 문의 갯수

		//public int ETS_STAT_10 { get; set; }	//입고대기
		//public int ETS_STAT_20 { get; set; }	//입고
		public int ETS_STAT_30 { get; set; }	//출고
		public int ETS_STAT_100 { get; set; }   //비행기 출발
		public int ETS_STAT_200 { get; set; }   //비행기 도착
		//public int ETS_STAT_300 { get; set; }   //통관 진행중
		//public int ETS_STAT_400 { get; set; }   //통관 완료
		public int ETS_STAT_500 { get; set; }   //배송 시작
		public int ETS_STAT_700 { get; set; }   //배송 완료


		//모델 생성자
		public HomeModels()
		{
			this.ETM_NOTICE = new List<ETM_LST>();
			this.EST_NOTICE = new List<EST_LST>();
			
			this.GOODS_CNT = 0;
			this.GODS_STOCK = 0;
			this.CS_CNT = 0;
			
			//this.ETS_STAT_10 = 0;
			//this.ETS_STAT_20 = 0;
			this.ETS_STAT_30 = 0;
			this.ETS_STAT_100 = 0;
			this.ETS_STAT_200 = 0;
			//this.ETS_STAT_300 = 0;
			this.ETS_STAT_500 = 0;
			this.ETS_STAT_700 = 0;

		}

		//ETOMARS 공지사항 최근 5개 셀렉트 리스트용 클래스
		public class ETM_LST
		{
			public string SEQNO { get; set; }
			public string TITLE { get; set; }     
			public string GET_DATE { get; set; }
		}


		//STATION 공지사항 최근 5개 셀렉트 리스트용 클래스
		public class EST_LST
		{
			public string SEQNO { get; set; }
			public string TITLE { get; set; }     
			public string WRITER { get; set; }     
			public string GET_DATE { get; set; }
		}
	}
}