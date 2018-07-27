using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_ese.Models_Act.Comm;

namespace web_ese.Models_Act.Cost
{
	public class CostIndexModels : SeachModel
	{
		//뷰
		public EstShippingFee Item { get; set; }  // 출고타입별 배송비 관리(무게별 설정) 

		//리스트 
		public List<EstShippingFee> Items { get; set; }  // 출고타입별 배송비 관리(무게별 설정) 

		//도착국가 셀렉트 박스 데이터 세팅
		public List<schTypeArray> nationArray { get; set; }

		//STATION 셀렉트 박스 데이터 세팅
		public List<schTypeArray> stationArray { get; set; }

		//출고타입 셀렉트 박스 데이터 세팅
		public List<schTypeArray> typeArray { get; set; }

		public string nation	{ get; set; }       //도착국가
		public string type		{ get; set; }		//출고타입
		public string station	{ get; set; }		//STATION

		//모델 생성자
		public CostIndexModels()
		{
			this.Item = new EstShippingFee();
			this.Items = new List<EstShippingFee>();
			this.nationArray = new List<schTypeArray>();
			this.stationArray = new List<schTypeArray>();
			this.typeArray = new List<schTypeArray>();
		}

	}
}