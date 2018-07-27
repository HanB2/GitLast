using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web_ese.Models_Act.Comm
{
	public class CommonModel
	{
		public string setPaging(PagingModel model)
		{
			int pageCnt = 5; //페이지 블럭 갯수

			int startPage = 0;
			int endPage = 0;

			startPage = (((model.page - 1) / pageCnt) * pageCnt) + 1;
			endPage = startPage + pageCnt - 1;

			if (endPage > model.pageTotNum)
				endPage = model.pageTotNum;

			string resultHtml = "";
			resultHtml += "<div class='text-center'>";
			resultHtml += "<ul class='pagination pagination-lg'>";

			if (startPage == 1)
			{
				resultHtml += "<li><a href='javascript: void(0);'><i class='fa fa-chevron-left'></i></a></li>";
			}
			else
			{
				resultHtml += "<li><a href=" + setJsStr(startPage - 1) + "><i class='fa fa-chevron-left'></i></a></li>";
			}


			for (int i = startPage; i <= endPage; i++)
			{
				if (model.page == i)
				{
					resultHtml += "<li class='active'><a href='javascript: void(0);'> " + i + " </a>";
				}
				else
				{
					resultHtml += "<li><a href=" + setJsStr(i) + "> " + i + " </a>";
				}
			}

			if (endPage + 1 > model.pageTotNum)
			{
				resultHtml += "<li><a href='javascript: void(0);'><i class='fa fa-chevron-right'></i></a></li>";
			}
			else
			{
				resultHtml += "<li><a href=" + setJsStr(endPage + 1) + "><i class='fa fa-chevron-right'></i></a></li>";
			}

			resultHtml += "</ul>";
			resultHtml += "</div>";

			//resultHtml += "<script> function movPage(page){	$('#page').val(page);	$('#listForm').submit(); } </script>";

			return resultHtml;
		}

		public string setJsStr(int page)
		{
			return "javascript:movPage('" + page + "');";
		}

	}



	//페이징 함수를 사용하기 위한 모델
	public class PagingModel
	{
		public int page { get; set; }
		public int pageNum { get; set; }
		public int pageTotNum { get; set; }
		public int totCnt { get; set; }
		public int startCnt { get; set; }

	}

	//셀렉트 박스 만들기 용 모델
	public class schTypeArray
	{
		public string opt_key { get; set; }   //공지유형
		public string opt_value { get; set; }   //공지유형
	}

	//검색과 페이징이 있는 모든 모델에서 확장 받아 사용 하는 모델
	public class SeachModel
	{
		//검색 조건
		public string schType { get; set; }   //공지유형
		public string schSdt { get; set; }   //등록일자 (시작일)
		public string schEdt { get; set; }   //등록일자 (종료일)
		public string schTypeTxt { get; set; }   //검색조건
		public string schTxt { get; set; }   //검색어
		public string schTxt2 { get; set; }   //검색어 2번째(msh 추가)
		//ESE_CODE 공통적으로 사용 하는 구분 코드
		public string ESE_CODE { get; set; }   //검색어

		//정렬
		public string sortKey { get; set; }    //정렬

		//act_type (서브밋 결과가 여러가지일 경우 구분을 위해 사용 ex: insert or update ...)
		//act_key  (서브밋 시 키가 사용될 경우 사용)
		public string act_type { get; set; }
		public int act_key { get; set; }

		//정렬 Array
		public List<schTypeArray> pageNumArray = new List<schTypeArray> {
			new schTypeArray {      opt_key = "10",        opt_value = "10"    },
			new schTypeArray {      opt_key = "50",        opt_value = "50" },
			new schTypeArray {      opt_key = "100",        opt_value = "100" }
		};

	}

	public class set_File
	{
		public HttpPostedFileBase FILE { get; set; }
	}
}