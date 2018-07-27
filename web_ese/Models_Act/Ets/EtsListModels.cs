using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ets
{
    public class EtsListModels : SeachModel
	{

		public PagingModel Paging { get; set; }  //페이징
		public List<ETS_LIST_ITEM> Items { get; set; }  // //리스트 	


		//뷰
		public ConfShippingCountry Item { get; set; }

		//국가
		public ConfAirpot NationItem { get; set; }
		public List<ConfAirpot> NationItems { get; set; }

		//조회
		public string schSNation { get; set; }   //출발 국가
		public string schStation { get; set; }  //검색 스테이션
		public string schENation { get; set; }   //도착 국가
		public string schStat { get; set; }   //상태

		public List<schTypeArray> arraySNation { get; set; }
		public List<schTypeArray> arrayStation { get; set; }
		public List<schTypeArray> arrayENation { get; set; }


		//검색조건
		public List<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "WAYBILLNO",        opt_value = "ETS NO" },
			new schTypeArray {      opt_key = "ORDERNO1",        opt_value = "주문번호" },
			new schTypeArray {      opt_key = "DELVNO",        opt_value = "현지 택배 번호" }
		};


		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "WAYBILLNO",        opt_value = "ETS NO" },
			new schTypeArray {      opt_key = "DELVNO",        opt_value = "현지 택배 번호" }
		};


		//상태 셀렉트 박스 데이터 세팅 
		public IEnumerable<schTypeArray> arrayStat = new List<schTypeArray> {
			new schTypeArray {      opt_key = "",        opt_value = "전체"    },
			new schTypeArray {      opt_key = "0",        opt_value = "배송중" },
			new schTypeArray {      opt_key = "1",        opt_value = "배송 완료" }

		};


		//기본정보
		public string BaseEstCode { get; set; }                 //ESE 소속 스테이션
		public string BaseEstName { get; set; }                 //ESE 소속 스테이션명
		public string BaseEseCode { get; set; }                 //ESE 코드
		public string BaseNationCode { get; set; }              //EST 국가
		public string BaseNationName { get; set; }              //EST 국가명		

		public string selChk { get; set; }

		public EtsListModels()
		{
			this.Paging = new PagingModel();
			this.Items = new List<ETS_LIST_ITEM>();
			this.NationItem = new ConfAirpot();
			this.NationItems = new List<ConfAirpot>();
			this.arraySNation = new List<schTypeArray>();
			this.arrayStation = new List<schTypeArray>();
			this.arrayENation = new List<schTypeArray>();
		}



		//리스트 삭제 문자열
		public string delChk { get; set; }

		//배송 상태 조회 리스트용 클래스
		public class ETS_LIST_ITEM
		{
			public string WAYBILLNO { get; set; }       //ETS NO

			public string DEP_NATION_CODE { get; set; } //출발국가 코드 (원본데이터)
			public string DEP_NATION_NAME { get; set; } //출발국가 이름

			public string NATION_CODE { get; set; } //도착국가 코드(원본데이터)
			public string NATION_NAME { get; set; } //도착국가 이름

			public string ORDERNO1 { get; set; }    //주문번호
			public string DELVNO { get; set; }      //현지택배번호
			public double DELVFEE { get; set; }     //배송비

			public int STATUS { get; set; }         //상태 원래 값
			public string STATUS_TEST { get; set; } //상태 문자열 변경 할 예정

			public string UPLOADYMD { get; set; }   //등록일자

			public string MEMO { get; set; }    //메모

		}

	}
}