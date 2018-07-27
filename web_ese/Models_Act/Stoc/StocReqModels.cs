using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Stoc
{
    public class StocReqModels : SeachModel
	{

		//기본정보
		public string BaseEstCode { get; set; }                 //ESE 소속 스테이션
		public string BaseEstName { get; set; }                 //ESE 소속 스테이션명
		public string BaseEseCode { get; set; }                 //ESE 코드
		public string BaseNationCode { get; set; }              //EST 국가
		public string BaseNationName { get; set; }              //EST 국가명
		public string BaseEseName { get; set; }              
		public string BaseEseTel { get; set; }              
		public string BaseAddr { get; set; }
		
		//상품 리스트 문자열
		public string goodsLst { get; set; }	

		//AJAX 데이터
		public string AjaxEstCode { get; set; }
		public string AjaxBarCode { get; set; }
		public string AjaxEseCode { get; set; }

		//바코드 체크용
		public OrdGoods InItem { get; set; }

		//폼전송용 임시
		public string GET_EST_CODE { get; set; }
		public string GET_INPUT_TYPE { get; set; }

 


		//정보
		public StcInReq Item { get; set; }				//뷰
		public List<StcInReqGoods> Items { get; set; }  //뷰 in goods data
		public List<OrdGoods> InItems { get; set; }		//뷰 in goods data
		public List<StcInReq> ListItems { get; set; }   //리스트

		public PagingModel Paging { get; set; }         


		//리스트 삭제 문자열
		public string delChk { get; set; }

		//검색조건
		public string schNation { get; set; }   //검색 국가
		public string schStation { get; set; }  //검색 스테이션

		//리스트 검색 셀랙트 박스용
		public List<schTypeArray> arraySNation { get; set; }
		public List<schTypeArray> arrayStation { get; set; }

		public List<schTypeArray> schTypeArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "",        opt_value = "전체"    },
			new schTypeArray {      opt_key = "10",        opt_value = "입고요청"    },
			new schTypeArray {      opt_key = "20",        opt_value = "검수필요" },
			new schTypeArray {      opt_key = "25",        opt_value = "검수완료" },
			new schTypeArray {      opt_key = "30",        opt_value = "입고완료" },
			new schTypeArray {      opt_key = "5",        opt_value = "취소" }
		};


		public List<schTypeArray> schTypeTxtArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "",        opt_value = "선택"    },
			new schTypeArray {      opt_key = "aaa",        opt_value = "aaa"    },
			new schTypeArray {      opt_key = "bbb",        opt_value = "bbb" },
			new schTypeArray {      opt_key = "ccc",        opt_value = "ccc" }
		};

		public List<schTypeArray> inputTypeArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "0",        opt_value = "택배"    },
			new schTypeArray {      opt_key = "1",        opt_value = "퀵"    },
			new schTypeArray {      opt_key = "2",        opt_value = "셀프" }
		};


		// 정렬부분 세팅
		public List<schTypeArray> sortKeyArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "EST_CODE",        opt_value = "STATION" },
			new schTypeArray {      opt_key = "INPUT_STAT",        opt_value = "상태" }

		};


		//모델 생성자
		public StocReqModels()
		{
			this.Paging = new PagingModel();
			this.InItem = new OrdGoods();
			this.ListItems = new List<StcInReq>();
			this.Items = new List<StcInReqGoods>();
			this.Item = new StcInReq();
			this.InItems = new List<OrdGoods>();
			this.arraySNation = new List<schTypeArray>();
			this.arrayStation = new List<schTypeArray>();
		}


		public STOC_EXCEL Excel { get; set; }

		//엑셀 업로드/다운로드 용 클래스
		public class STOC_EXCEL
		{
			public bool result { get; set; }
			public string FileType { get; set; }
			public List<string> errList { get; set; }
			public set_File File { get; set; }  //HttpPostedFileBase 
		}
	}
}