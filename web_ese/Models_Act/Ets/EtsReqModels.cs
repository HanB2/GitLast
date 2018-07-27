using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Ets
{
    public class EtsReqModels : SeachModel
	{
		//오류 처리 문구
		public string err_str { get; set; }

		//AJAX 데이터
		public string AjaxEstCode { get; set; }
		public string AjaxEseCode { get; set; }
		public string AjaxBarCode { get; set; }
		public string AjaxNationCode { get; set; }
		public string AjaxReleaseCode { get; set; }

		//필요 기본 정보
		public string BaseEstCode { get; set; }                 //ESE 소속 스테이션
		public string BaseEstName { get; set; }                 //ESE 소속 스테이션명
		public string BaseEseCode { get; set; }					//ESE 코드
		public string BaseNationCode { get; set; }              //EST 국가
		public string BaseNationName { get; set; }              //EST 국가명
		public string BaseReleaseCode { get; set; }				//EST 배송타입
		public string BaseReleaseCost { get; set; }             //EST 배송요금	- 예상 배송비용 측정에 사용

		public string BaseReleaseWeightUnit { get; set; }		//EST 무게단위
		public string BaseReleaseDimUnit { get; set; }          //EST 길이단위
		public string BaseWeightDivide_KG { get; set; }         //EST 부피 무게 계산 값
		public string BaseWeightDivide_LB { get; set; }         //EST 부피 무게 계산 값

		public string BaseEseName { get; set; }					//회원 기본정보 이름	-- 센더명으로 지정
		public string BaseEseTel { get; set; }					//회원 기본정보 전화번호
		public string BaseEseAddr { get; set; }					//회원 기본정보 주소


		public List<schTypeArray> arraySNation { get; set; }	//출발국가
		public List<schTypeArray> arrayStation { get; set; }	//출발 스테이션
		public List<schTypeArray> arrayENation { get; set; }	//도착국가
		public List<schTypeArray> arrayCost { get; set; }       //배송타입
		public List<schTypeArray> arrayCurrencyGoods { get; set; }       //배송타입

		//우편번호 관련
		//중국 성 시 구
		public string BaseCnAddr1 { get; set; }                 //성
		public string BaseCnAddr2 { get; set; }                 //시
		public string BaseCnAddr3 { get; set; }                 //구
		public List<schTypeArray> arrayCnAddr1 { get; set; }    //성 리스트
		public List<schTypeArray> arrayCnAddr2 { get; set; }    //시 리스트
		public List<schTypeArray> arrayCnAddr3 { get; set; }    //구 리스트

		//미국 1단계 2단계
		public string BaseUsAddr1 { get; set; }                 //1단계
		public string BaseUsAddr2 { get; set; }                 //2단계
		public List<schTypeArray> arrayUsAddr1 { get; set; }    //1단계 리스트
		public List<schTypeArray> arrayUsAddr2 { get; set; }    //2단계 리스트






		//상품 정보 JSON 데이터 타입으로 가져오기
		public string goodsLst { get; set; }



		//배송 정보
		public OrdMaster Item { get; set; }

		//개별 상품 
		public List<OrdGoods> Items { get; set; }


		//바코드 체크용
		public OrdGoods InItem { get; set; }

		//ajax retuen 값 전용
		public List<schTypeArray> ajaxArray { get; set; }

		public EtsReqModels(){
			Item = new OrdMaster();
			Items = new List<OrdGoods>();
			InItem = new OrdGoods();
			ajaxArray = new List<schTypeArray>();

			arraySNation = new List<schTypeArray>();
			arrayStation = new List<schTypeArray>();
			arrayENation = new List<schTypeArray>();
			arrayCost = new List<schTypeArray>();
			arrayCurrencyGoods = new List<schTypeArray>();

			arrayCnAddr1 = new List<schTypeArray>();
			arrayCnAddr2 = new List<schTypeArray>();
			arrayCnAddr3 = new List<schTypeArray>();
			arrayUsAddr1 = new List<schTypeArray>();
			arrayUsAddr2 = new List<schTypeArray>();
		}

	}
}