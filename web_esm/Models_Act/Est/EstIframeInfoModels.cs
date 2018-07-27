using comm_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using web_esm.Models_Act.Comm;

namespace web_esm.Models_Act.Est
{
	public class EstIframeInfoModels : SeachModel
	{
		public string viewEstCode { get; set; }   //Station

		//뷰
		public EsmStation Item { get; set; }

		//리스트 
		public List<EsmStation> Items { get; set; }

		//모델 생성자
		public EstIframeInfoModels()
		{	
			this.Item = new EsmStation();
			this.Items = new List<EsmStation>();
			this.stationArray = new List<schTypeArray>();
			this.nationArray = new List<schTypeArray>();
			this.weightArray = new List<schTypeArray>();
		}

		//국가
		public List<schTypeArray> nationArray { get; set; }

		//라디오버튼
		public bool RadioBoxPop { get; set; }
		//라디오버튼
		public bool RadioBoxSummerT { get; set; }

		//무게 단위
		public List<schTypeArray> weightArray { get; set; }

		//STATION 셀렉트 박스 데이터 세팅
		public List<schTypeArray> stationArray { get; set; }
		public string station { get; set; }   //Station
		public string WEIGHT_UNIT { get; set; } //weigh_unit
	}
}