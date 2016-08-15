/*行业选择弹出层填充数据*/
function trade_filldata(fillID, data_resources, showID, resultlist, resultshowID, resultID, dir) {
	var tradhtm = '';
	var sourse_length = parseInt(data_resources.length);
	var rows = 0;
	var subscriptnum = 0;
	if((sourse_length%4) == 0) {
		rows = sourse_length / 4;
	} else {
		rows = (sourse_length / 4) + 1;
	}
	for (var i = 0; i < rows; i++) {
		tradhtm += '<tr>';
		for (var j = 0; j < 4; j++) {
			if (data_resources[subscriptnum]) {
				var trad_array = data_resources[subscriptnum].split(",");
				tradhtm += '<td><label><input type="checkbox" data="'+trad_array[0]+','+trad_array[1]+'" name="item-list" class="input-checkbox">'+trad_array[1]+'</label></td>';
				subscriptnum ++;
			};
		};
		tradhtm += '</tr>';
	};
	$(fillID).html(tradhtm);
	// 恢复选中
	if ($(resultID).val().length > 0) {
		var idarray = $(resultID).val().split(",");
		$.each(idarray, function(index, val) {
			$(fillID+" :checkbox").unbind().each(function() {
				var boxdata_array = $(this).attr("data").split(",");
				if(val == boxdata_array[0]) {
					$(this).attr('checked',true);
				}
			});
		});
		copyitem();
		copy_selected();
		isexceed($(fillID+" :checkbox[checked]").length);
		// 单选
		$(fillID+" label :checkbox").unbind().live("click",function(event) {
			if ($(this).find(":checkbox").attr('checked')) {
				$(this).find(":checkbox").attr('checked',false);
				isexceed($(fillID+" :checkbox[checked]").length);
			} else {
				$(this).find(":checkbox").attr('checked',true);
				isexceed($(fillID+" :checkbox[checked]").length);
			}
		});
	};
	// 单选
	$(fillID+" label :checkbox").unbind().live("click",function(event) {
		if ($(this).find(":checkbox").attr('checked')) {
			$(this).find(":checkbox").attr('checked',false);
			isexceed($(fillID+" :checkbox[checked]").length);
		} else {
			$(this).find(":checkbox").attr('checked',true);
			isexceed($(fillID+" :checkbox[checked]").length);
		}
	});
	// 复制选中项目
	function copyitem() {
		var resulthtm = '';
		$(fillID+" :checkbox[checked]").unbind().each(function(index) {
			var result_data_array = $(this).attr("data").split(",");
			resulthtm += '<span data-code="'+result_data_array[0]+','+result_data_array[1]+'">'+result_data_array[1]+'<i class="data-icon data-icon-close"></i></span>';
		});
		$(resultlist).html(resulthtm);
		var spi = $(resultlist + " span");
		spi.unbind().bind('click', function() {
			var spidata = $(this).attr("data-code"),
				apid_array = splitdata(spidata);
			$(fillID+" :checkbox[checked]").unbind().each(function(index) {
				var result_data_array = $(this).attr("data").split(",");
				if (apid_array[0] == result_data_array[0]) {
					$(this).attr('checked',false);
					isexceed($(fillID+" :checkbox[checked]").length);
				}
			});
			copyitem();
		});
		$("#arstrade").html($(fillID+" :checkbox[checked]").length);
		if ($(fillID+" :checkbox[checked]").length > 0) {
			$(".cla").show();
			$(".cla").click(function() {
				$(fillID+" :checkbox").unbind().each(function(index) {
					$(this).attr('disabled',false);
					$(this).attr('checked',false);
				});
				copyitem();
			})
		} else {
			$(".cla").hide();
		}
	}
	// 复制选中 赋值
	function copy_selected() {
		var selectedhtm = "";
		var selectedid_array = new Array();
		var selectedcn_array = new Array();
		$(fillID+" :checkbox[checked]").unbind().each(function(index) {
			var data_array = $(this).attr("data").split(",");
			selectedid_array[index]=data_array[0];
			selectedcn_array[index]=data_array[1];
		});
		$("#tradText").html(selectedcn_array.join(","));
		$(resultID).val(selectedid_array.join(","));
		$("#trade_cn").val(selectedcn_array.join(","));
		$("#tradText").parent().find('.input_text_200_bg').removeClass('error');
		$("#tradText").parent().next().find('.error').hide();
	}
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
	/*判断选中的数量是否超出最大限制*/
	function isexceed(num) {
		if(num >= 3) {
			$(fillID+" :checkbox").each(function(index, el) {
				if (!$(this).attr('checked')) {
					$(this).attr('disabled',"disabled");
				}
			});
		} else {
			$(fillID+" :checkbox").each(function(index, el) {
				$(this).attr('disabled',false);
			});
		}
		copyitem();
	}
	// 点击确定
	$("#tr-selector-save").click(function() {
		copy_selected();
		$('.aui_outer').hide();
		$(".ucc-default").removeClass('aui_is_show');
	});
}
/*所学专业选择弹出层填充数据*/
function major_filldata(fillID, data_resourcesP, data_resources, navID, showID, resultlist, resultshowID, resultID, dir) {
	var tradhtm = '',marorPhtm = '';
	$.each(data_resourcesP, function(indexp, valp) {
		var pjob_array = valp.split(",");
		marorPhtm += '<li code="'+pjob_array[0]+'"';
		indexp == 0 ? marorPhtm += ' class="tnon">' : marorPhtm += '>';
		marorPhtm += pjob_array[1]+'</li>';
		var major_sourseArray = data_resources[pjob_array[0]].split("|"),
			sourse_length = parseInt(major_sourseArray.length);
		var rows = 0;
		var subscriptnum = 0;
		if((sourse_length%4) == 0) {
			rows = sourse_length / 4;
		} else {
			rows = parseInt(sourse_length / 4) + 1;
		}
		tradhtm += '<tbody class="major_num_t">';
		for (var i = 0; i < rows; i++) {
			tradhtm += '<tr>';
			for (var j = 0; j < 4; j++) {
				if (major_sourseArray[subscriptnum]) {
					var trad_array = major_sourseArray[subscriptnum].split(",");
					tradhtm += '<td><label data="'+pjob_array[0]+','+trad_array[0]+','+trad_array[1]+'">'+trad_array[1]+'</label></td>';
					subscriptnum ++;
				}
			}
			tradhtm += '</tr>';
		}
		tradhtm += '</tbody>';
	});
	$(navID).html(marorPhtm);
	$(fillID).html(tradhtm);
	$(".major_num_t").eq(0).show().siblings('.major_num_t').hide();
	$(navID+" li").unbind().bind('click',function(){
		$(navID+" li").removeClass('tnon');
		$(this).addClass('tnon');
		var ind = $(navID+" li").index(this);
		$(".major_num_t").eq(ind).show().siblings('.major_num_t').hide();
	});
	// 恢复选中
	if ($(resultID).val().length > 0) {
		var rgsid = $(resultID).val();
		$(fillID+" label").each(function(index, el) {
			var rgsdrelArray = $(this).attr("data").split(",");
			if (rgsid == rgsdrelArray[1]) {
				$(this).addClass('gselectra');
				var prgsid = rgsdrelArray[0];
				$(navID+" li").each(function(index, el) {
					var rgsdrelVal = $(this).attr("code");
					if (prgsid == rgsdrelVal) {
						$(navID+" li").removeClass('tnon');
						$(this).addClass('tnon');
						$(".major_num_t").eq(index).show().siblings('.major_num_t').hide();
					}
				})
			}
		});
	};
	// 单选
	$(fillID+" label").unbind().live("click",function(event) {
		$(fillID+" label").removeClass('gselectra');
		$(this).addClass('gselectra');
		var checkMsgArray = $(this).attr("data").split(",");
		$("#majorText").html(checkMsgArray[2]);
		$("#major_cn").val(checkMsgArray[2]);
		$("#major").val(checkMsgArray[1]);
		$("#major").parent().find('.input_text_200_bg').removeClass('error');
		$("#major").parent().next().find('.error').hide();
		$('.aui_outer').hide();
		$(".ucc-default").removeClass('aui_is_show');
	});
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
}
/*职位选择弹出层填充数据*/
function job_filldata(fillID, data_resourcesP, data_resources, resultlist, showID, resultshowID, resulthidId, dir) {
	var jobhtm = '', result_datapool = new Array();
	$.each(data_resourcesP, function(indexp, valp) {
		 var pjob_array = valp.split(",");
		 	jobhtm += '<div class="data-row data-row-odd item-list clearfix">';
		 jobhtm += '<div class="data-row-side">'+pjob_array[1]+'</div>';
		 jobhtm += '<div class="data-row-side-r"><ul>';
		 var job_array = data_resources[pjob_array[0]].split("|");
		 $.each(job_array, function(index, val) {
		 	 var joblist_array = val.split(",");
		 	 jobhtm += '<li><a title="'+joblist_array[1]+'" href="javascript:;" data="'+pjob_array[0]+'.'+joblist_array[0]+'.0,'+joblist_array[1]+'" rel="'+pjob_array[0]+'.'+joblist_array[0]+'" class="cat"><i class="data-icon data-icon-expend"></i>'+joblist_array[1]+'</a><a href="javascript:;" class="cat-touch"><label title="'+joblist_array[1]+'"><input type="checkbox" data="'+pjob_array[0]+'.'+joblist_array[0]+'.0,'+joblist_array[1]+'" rel="'+pjob_array[0]+'.'+joblist_array[0]+'" class="checkbox" name="LocalDataMultiC">'+joblist_array[1]+'</label></a></li>';
		 });
		 jobhtm += '</ul></div>';
		 jobhtm += '</div>';
	});
	$(fillID).html(jobhtm);
	// 全部恢复
	if ($(resulthidId).val().length > 0) {
		var recoverid_array = $(resulthidId).val().split(","),
			recovercn_array = new Array();
		$.each(recoverid_array, function(index, val) {
			var vspArray = val.split("."),
				vue = vspArray[2];
			if (vue == 0) {
				if (data_resources[vspArray[0]]) {
					var vueArray = data_resources[vspArray[0]].split("|");
					$.each(vueArray, function(indexso, valso) {
						var vuesoArray = valso.split(",");
						if (vspArray[1] == vuesoArray[0]) {
							recovercn_array.push(vuesoArray[1]);
						}
					});
				}
			} else {
				if (data_resources[vspArray[1]]) {
					var vuesArray = data_resources[vspArray[1]].split("|");
					$.each(vuesArray, function(indexson, valson) {
						var vuesonArray = valson.split(",");
						if (vue == vuesonArray[0]) {
							recovercn_array.push(vuesonArray[1]);
						}
					});
				}
			}
		});
		for (var i = 0; i < recoverid_array.length; i++) {
			result_datapool[i] = recoverid_array[i]+","+recovercn_array[i];
		};
		copyitem();
		copy_selected();
		var cata_array = $(fillID + " a.cat");
		$.each(cata_array, function() {
			var catcobj = $(this), catcheckedid_array = splitdata(catcobj.attr("data"));
			$.each(result_datapool, function(indexdp,valdp) {
				var rdatapool_array = splitdata(valdp);
				if (catcheckedid_array[1]==rdatapool_array[1] && rdatapool_array[2] !=0) {
					catcobj.addClass('cat-checked');
				} else if(catcheckedid_array[1]==rdatapool_array[1] && rdatapool_array[2] ==0) {
					catcobj.addClass('cat-checked');
					catcobj.next().find("input").attr("checked", !0);
				}
			})
		});
	}
	$(fillID + " a.cat").unbind().on("click",function() {
		var ace = $(this), jobarrayid_array = ace.attr("rel").split(".");
		if (data_resources[jobarrayid_array[1]]) { // 判断是否有三级分类
			var tjobhtm = '<div class="data-sub"><table cellpadding="0" cellspacing="0"><tbody>';
			var tjob_array = data_resources[jobarrayid_array[1]].split("|");
			var sourse_length = parseInt(tjob_array.length);
			var rows = 0;
			var subscriptnum = 0;
			if((sourse_length%2) == 0) {
				rows = sourse_length / 2;
			} else {
				rows = (sourse_length / 2) + 1;
			}
			for (var i = 0; i < rows; i++) {
				tjobhtm += '<tr>';
				for (var j = 0; j < 2; j++) {
					if (tjob_array[subscriptnum]) {
						var tjoblist_array = tjob_array[subscriptnum].split(",");
						tjobhtm += '<td><a class="cat" data="'+jobarrayid_array[0]+'.'+jobarrayid_array[1]+'.'+tjoblist_array[0]+','+tjoblist_array[1]+'" href="javascript:;"><label title="'+tjoblist_array[1]+'"><input type="checkbox" class="checkbox" rel="'+jobarrayid_array[0]+'.'+jobarrayid_array[1]+'.'+tjoblist_array[0]+'" data="'+jobarrayid_array[0]+'.'+jobarrayid_array[1]+'.'+tjoblist_array[0]+','+tjoblist_array[1]+'" />'+tjoblist_array[1]+'</label></a></td>';
						subscriptnum ++;
					}
				};
				tjobhtm += '</tr>';
			};
			tjobhtm += '</tbody></table></div>';
			$(fillID).append(tjobhtm);
			var d = $(".data-sub"), l = $(this).closest("li");
			var g = l.find(".cat-touch input"), s = d.find("input");
			// 判断是否有选中
			var rco_resultlist = $(resultlist + " span");
			if (rco_resultlist.length > 0) {
				$.each(rco_resultlist, function() {
					var dsc = $(this).attr("data-code"),
					 	dscode_array = splitdata(dsc);
					if(dscode_array[2] == 0) {
						$.each(g, function() {
							if (dsc == $(this).attr("data")) {
							 	var idsc = $(this).is(':checked');
							 	$(this).attr("checked", idsc);
							 	s.prop({"checked":idsc, "disabled":idsc});
							}
						})
					} else {
						$.each(s, function() {
							if (dsc == $(this).attr("data")) {
							 	$(this).attr("checked", !0);
							}
						})
					}
				})
			} else {
				g.prop("checked", false);
			}
			var pleft = $(fillID).offset().left,
				ptop = $(fillID).offset().top,
				pwidth = $(fillID).outerWidth(),
				pheight = $(fillID).outerHeight(),
				ileft = $(this).offset().left,
				itop = $(this).offset().top,
				iwidth = $(this).outerWidth(),
				iheight = $(this).outerHeight(),
				dwidth = d.outerWidth(),
				dheight = d.outerHeight(),
				comparetop = parseInt(itop - ptop),
				dcleft = parseInt(ileft - pleft);
			var pbv = 2, bpv = 1;
			$.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());
			if($.browser.msie) {
				pbv = 1;
				bpv = 2;
			}
			l.addClass('cat-active');
			var n = $(this).next();
			pheight - dheight - iheight < comparetop ? (n.removeClass("sub").addClass("sup"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth))) : dcleft),top:(comparetop + iheight)-dheight-iheight + pbv})) : (n.removeClass("sup").addClass("sub"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth))) : dcleft),top:(comparetop + iheight) - bpv}));
			n.bind('mouseenter', function() {
				var e = $(this).closest("li");
				e.attr("data-active", !0)
			}).bind('mouseleave', function() {
				var a = $(this).closest("li");
				a.removeAttr("data-active"), setTimeout(function() {
					a.attr("data-active") || ($(fillID).find(".data-sub").remove(), a.removeClass("cat-active"))
				}, 200)
			});
			d.bind('mouseenter', function() {
				l.attr("data-active", !0)
			}).bind('mouseleave', function() {
				l.removeAttr("data-active"), setTimeout(function() {
					l.attr("data-active") || (d.remove(), l.removeClass("cat-active"))
				}, 200)
			});
			g.unbind('click').bind('click', function() {
				var ic = $(this).is(':checked'), gdata = $(this).attr("data");
				s.prop({"checked":ic, "disabled":ic});
				if (ic) {
					var gdata_array = splitdata(gdata);
					for (var i = 0; i < result_datapool.length; i++) {
						if (result_datapool[i]) {
							var icgdata_array = splitdata(result_datapool[i]);
							if (gdata_array[1] == icgdata_array[1]) {
								result_datapool.splice(i,1);
								i = -1;
							}
						}
					}
					result_datapool.push(gdata);
					if(isexceed()) {
						ace.addClass('cat-checked');
					} else {
						$(this).attr("checked",false);
						result_datapool.splice($.inArray(gdata,result_datapool),1);
					}
				} else {
					result_datapool.splice($.inArray(gdata,result_datapool),1);
					ace.removeClass('cat-checked');
				}
				copyitem();
			});
			s.unbind('click').bind('click', function() {
				var sic = $(this).is(':checked'), sdata = $(this).attr("data");
				if (sic) {
					result_datapool.push(sdata);
					isexceed() ? ace.addClass('cat-checked') : result_datapool.splice($.inArray(sdata,result_datapool),1);
				} else {
					result_datapool.splice($.inArray(sdata,result_datapool),1);
					var chnum = d.find(' :checkbox[checked]').length;
					if (chnum <= 0) {
						ace.removeClass('cat-checked');
					}
				}
				copyitem();
			});
		} else{
			var l = $(this).closest("li");
			var g = l.find(".cat-touch input");
			// 判断是否有选中
			var rco_resultlist = $(resultlist + " span");
			if (rco_resultlist.length > 0) {
				$.each(rco_resultlist, function() {
					var dsc = $(this).attr("data-code"),
					 	dscode_array = splitdata(dsc);
					if(dscode_array[2] == 0) {
						$.each(g, function() {
							if (dsc == $(this).attr("data")) {
							 	var idsc = $(this).is(':checked');
							 	$(this).attr("checked", idsc);
							}
						})
					}
				})
			} else {
				g.prop("checked", false);
			}
			l.addClass('cat-active');
			var n = $(this).next();
			n.bind('mouseenter', function() {
				var e = $(this).closest("li");
				e.attr("data-active", !0)
			}).bind('mouseleave', function() {
				var a = $(this).closest("li");
				a.removeAttr("data-active"), setTimeout(function() {
					a.attr("data-active") || ($(fillID).find(".data-sub").remove(), a.removeClass("cat-active"))
				}, 200)
			});
			g.unbind('click').bind('click', function() {
				var ic = $(this).is(':checked'), gdata = $(this).attr("data");
				if (ic) {
					result_datapool.push(gdata);
					if(isexceed()) {
						ace.addClass('cat-checked');
					} else {
						$(this).attr("checked",false);
						result_datapool.splice($.inArray(gdata,result_datapool),1);
					}
				} else {
					result_datapool.splice($.inArray(gdata,result_datapool),1);
					ace.removeClass('cat-checked');
				}
				copyitem();
			});
		};
	});
	// 复制选中项目
	function copyitem() {
		var resulthtm = '';
		$.each(result_datapool, function(indexrd,valrd) {
			var result_data_array = valrd.split(",");
			resulthtm += '<span data-code="'+result_data_array[0]+','+result_data_array[1]+'">'+result_data_array[1]+'<i class="data-icon data-icon-close"></i></span>';
		});
		$(resultlist).html(resulthtm);
		var spi = $(resultlist + " span");
		spi.unbind().bind('click', function() {
			var spidata = $(this).attr("data-code"),
				apid_array = splitdata(spidata);
			result_datapool.splice($.inArray(spidata,result_datapool),1);
			var catchecked_array = $(fillID + " a.cat-checked");
			$.each(catchecked_array, function() {
				var catcobj = $(this), catcheckedid_array = splitdata(catcobj.attr("data"));
				if(apid_array[1]==catcheckedid_array[1] && apid_array[2]==0){
					catcobj.removeClass('cat-checked');
					catcobj.next().find("input").attr("checked", false);
				} else if (apid_array[1]==catcheckedid_array[1] && apid_array[2]!=0) {
					var rdanum = 0;
					$.each(result_datapool, function(indexdp,valdp) {
						var rdatapool_array = splitdata(valdp);
						if (apid_array[1]==rdatapool_array[1] && apid_array[2]!=0) {
							rdanum ++;
						}
					});
					if (rdanum <= 0) {
						catcobj.removeClass('cat-checked');
					}
				}
			});
			copyitem();
		});
		$("#ars").html(result_datapool.length);
		if (result_datapool.length > 0) {
			$(".cla").show();
			$(".cla").click(function() {
				result_datapool.splice(0,result_datapool.length);
				var catchecked_array = $(fillID + " a.cat-checked");
				$.each(catchecked_array, function(index, val) {
					$(this).removeClass('cat-checked');
				});
				copyitem();
			})
		} else {
			$(".cla").hide();
		}
	}
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
	// 判断选择数量是否超出
	function isexceed() {
		if(result_datapool.length > 3) {
			alert("最多可选3个");
			return false;
		} else {
			return true;
		}
	}
	// 复制选中 赋值
	function copy_selected() {
		var selectedhtm = "";
		var selectedid_array = new Array();
		var selectedcn_array = new Array();
		for (var i = 0; i < result_datapool.length; i++) {
			if (result_datapool[i]) {
				var data_array = result_datapool[i].split(",");
				selectedid_array[i]=data_array[0];
				selectedcn_array[i]=data_array[1];
			};
		};
		$("#jobText").html(selectedcn_array.join(","));
		$(resulthidId).val(selectedid_array.join(","));
		$("#intention_jobs").val(selectedcn_array.join(","));
		$("#jobText").parent().find('.input_text_200_bg').removeClass('error');
		$("#jobText").parent().next().find('.error').hide();
	}
	// 点击确定
	$("#jb-selector-save").click(function() {
		copy_selected();
		$('.aui_outer').hide();
		$(".ucc-default").removeClass('aui_is_show');
	});
}
/*地区选择弹出层填充数据*/
function city_filldata(fillID, data_resourcesP, data_resources, resultlist, showID, resultshowID, resulthidId, dir) {
	var cityhtm = '', result_datapool = new Array();

	var sourse_city_length = parseInt(data_resourcesP.length);
	var rows_city = 0;
	var subscriptnum_city = 0;
	// 计算总行数
	if((sourse_city_length%5) == 0) {
		rows_city = sourse_city_length / 5;
	} else {
		if (sourse_city_length > 5*((sourse_city_length / 5) + 1)) {
			rows_city = (sourse_city_length / 5) + 1;
		} else {
			rows_city = (sourse_city_length / 5);
		}
	}
	for (var i = 0; i < rows_city; i++) {
		cityhtm += '<div class="data-row item-list data-row-nob clearfix">';
		cityhtm += '<div class="data-row-side-r615"><ul>';
		for (var j = 0; j < 5; j++) {
			if (data_resourcesP[subscriptnum_city]) {
				var citylist_array = data_resourcesP[subscriptnum_city].split(",");
				cityhtm += '<li><a title="'+citylist_array[1]+'" href="javascript:;" data="'+citylist_array[0]+'.0,'+citylist_array[1]+'" rel="'+citylist_array[0]+'.0" class="cat"><i class="data-icon data-icon-expend"></i>'+citylist_array[1]+'</a><a href="javascript:;" class="cat-touch"><label title="'+citylist_array[1]+'"><input type="checkbox" data="'+citylist_array[0]+'.0,'+citylist_array[1]+'" rel="'+citylist_array[0]+'.0" class="checkbox" name="LocalDataMultiC">'+citylist_array[1]+'</label></a></li>';
				subscriptnum_city ++;
			};
		};
		cityhtm += '</ul></div>';
		cityhtm += '</div>';
	};
	$(fillID).html(cityhtm);
	// 全部恢复
	if ($(resulthidId).val().length > 0) {
		var recoverid_array = $(resulthidId).val().split(","),
			recovercn_array = new Array();
		$.each(recoverid_array, function(index, val) {
			var vspArray = val.split("."),
				vue = vspArray[1];
			if (vue == 0) {
				$.each(data_resourcesP, function(indexso, valso) {
					var vuesoArray = valso.split(",");
					if (vspArray[0] == vuesoArray[0]) {
						recovercn_array.push(vuesoArray[1]);
					}
				});
			} else {
				if (data_resources[vspArray[0]]) {
					var vuesArray = data_resources[vspArray[0]].split("|");
					$.each(vuesArray, function(indexson, valson) {
						var vuesonArray = valson.split(",");
						if (vue == vuesonArray[0]) {
							recovercn_array.push(vuesonArray[1]);
						}
					});
				}
			}
		});
		for (var i = 0; i < recoverid_array.length; i++) {
			result_datapool[i] = recoverid_array[i]+","+recovercn_array[i];
		};
		copyitem();
		copy_selected();
		var cata_array = $(fillID + " a.cat");
		$.each(cata_array, function() {
			var catcobj = $(this), catcheckedid_array = splitdata(catcobj.attr("data"));
			$.each(result_datapool, function(indexdp,valdp) {
				var rdatapool_array = splitdata(valdp);
				if (catcheckedid_array[0]==rdatapool_array[0] && rdatapool_array[1] !=0) {
					catcobj.addClass('cat-checked');
				} else if(catcheckedid_array[0]==rdatapool_array[0] && rdatapool_array[1] ==0) {
					catcobj.addClass('cat-checked');
					catcobj.next().find("input").attr("checked", !0);
				}
			})
		});
	}
	$(fillID + " a.cat").unbind().on("click",function() {
		var ace = $(this), cityarrayid_array = ace.attr("rel").split(".");
		if (data_resources[cityarrayid_array[0]]) { // 判断是否有二级地区
			var tcityhtm = '<div class="data-sub"><table cellpadding="0" cellspacing="0"><tbody>';
			var tcity_array = data_resources[cityarrayid_array[0]].split("|");
			var sourse_length = parseInt(tcity_array.length);
			var rows = 0;
			var subscriptnum = 0;
			if((sourse_length%2) == 0) {
				rows = sourse_length / 2;
			} else {
				rows = (sourse_length / 2) + 1;
			}
			for (var i = 0; i < rows; i++) {
				tcityhtm += '<tr>';
				for (var j = 0; j <= 2; j++) {
					if (tcity_array[subscriptnum]) {
						var tcitylist_array = tcity_array[subscriptnum].split(",");
						tcityhtm += '<td><a class="cat" data="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+','+tcitylist_array[1]+'" href="javascript:;"><label title="'+tcitylist_array[1]+'"><input type="checkbox" class="checkbox" rel="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+'" data="'+cityarrayid_array[0]+'.'+tcitylist_array[0]+','+tcitylist_array[1]+'" />'+tcitylist_array[1]+'</label></a></td>';
						subscriptnum ++;
					}
				};
				tcityhtm += '</tr>';
			};
			tcityhtm += '</tbody></table></div>';
			$(fillID).append(tcityhtm);
			var d = $(".data-sub"), l = $(this).closest("li");
			var g = l.find(".cat-touch input"), s = d.find("input");
			// 判断是否有选中
			var rco_resultlist = $(resultlist + " span");
			if (rco_resultlist.length > 0) {
				$.each(rco_resultlist, function() {
					var dsc = $(this).attr("data-code"),
					 	dscode_array = splitdata(dsc);
					if(dscode_array[1] == 0) {
						$.each(g, function() {
							if (dsc == $(this).attr("data")) {
							 	var idsc = $(this).is(':checked');
							 	$(this).attr("checked", idsc);
							 	s.prop({"checked":idsc, "disabled":idsc});
							}
						})
					} else {
						$.each(s, function() {
							if (dsc == $(this).attr("data")) {
							 	$(this).attr("checked", !0);
							}
						})
					}
				})
			} else {
				g.prop("checked", false);
			}
			var pleft = $(fillID).offset().left,
				ptop = $(fillID).offset().top,
				pwidth = $(fillID).outerWidth(),
				pheight = $(fillID).outerHeight(),
				ileft = $(this).offset().left,
				itop = $(this).offset().top,
				iwidth = $(this).outerWidth(),
				iheight = $(this).outerHeight(),
				dwidth = d.outerWidth(),
				dheight = d.outerHeight(),
				comparetop = parseInt(itop - ptop),
				dcleft = parseInt(ileft - pleft);
			var pbv = 2, bpv = 1;
			$.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());
			if($.browser.msie) {
				pbv = 1;
				bpv = 2;
			}
			l.addClass('cat-active');
			var n = $(this).next();
			pheight - dheight - iheight < comparetop ? (n.removeClass("sub").addClass("sub"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth))) : dcleft),top:(comparetop + iheight) - bpv})) : (n.removeClass("sub").addClass("sub"), d.css({left:(pwidth-dwidth < dcleft ? (pwidth-dwidth-((pleft+pwidth)-(ileft+iwidth)))  : dcleft),top:(comparetop + iheight) - bpv}));
			n.bind('mouseenter', function() {
				var e = $(this).closest("li");
				e.attr("data-active", !0)
			}).bind('mouseleave', function() {
				var a = $(this).closest("li");
				a.removeAttr("data-active"), setTimeout(function() {
					a.attr("data-active") || ($(fillID).find(".data-sub").remove(), a.removeClass("cat-active"))
				}, 200)
			});
			d.bind('mouseenter', function() {
				l.attr("data-active", !0)
			}).bind('mouseleave', function() {
				l.removeAttr("data-active"), setTimeout(function() {
					l.attr("data-active") || (d.remove(), l.removeClass("cat-active"))
				}, 200)
			});
			g.unbind('click').bind('click', function() {
				var ic = $(this).is(':checked'), gdata = $(this).attr("data");
				s.prop({"checked":ic, "disabled":ic});
				if (ic) {
					var gdata_array = splitdata(gdata);
					for (var i = 0; i < result_datapool.length; i++) {
						if (result_datapool[i]) {
							var icgdata_array = splitdata(result_datapool[i]);
							if (gdata_array[0] == icgdata_array[0]) {
								result_datapool.splice(i,1);
								i = -1;
							}
						}
					}
					result_datapool.push(gdata);
					if(isexceed()) {
						ace.addClass('cat-checked');
					} else {
						$(this).attr("checked",false);
						result_datapool.splice($.inArray(gdata,result_datapool),1);
					}
				} else {
					result_datapool.splice($.inArray(gdata,result_datapool),1);
					ace.removeClass('cat-checked');
				}
				copyitem();
			});
			s.unbind('click').bind('click', function() {
				var sic = $(this).is(':checked'), sdata = $(this).attr("data");
				if (sic) {
					result_datapool.push(sdata);
					isexceed() ? ace.addClass('cat-checked') : result_datapool.splice($.inArray(sdata,result_datapool),1);
				} else {
					result_datapool.splice($.inArray(sdata,result_datapool),1);
					var chnum = d.find(' :checkbox[checked]').length;
					if (chnum <= 0) {
						ace.removeClass('cat-checked');
					}
				}
				copyitem();
			});
		} else{
			var l = $(this).closest("li");
			var g = l.find(".cat-touch input");
			// 判断是否有选中
			var rco_resultlist = $(resultlist + " span");
			if (rco_resultlist.length > 0) {
				$.each(rco_resultlist, function() {
					var dsc = $(this).attr("data-code"),
					 	dscode_array = splitdata(dsc);
					if(dscode_array[0] == 0) {
						$.each(g, function() {
							if (dsc == $(this).attr("data")) {
							 	var idsc = $(this).is(':checked');
							 	$(this).attr("checked", idsc);
							}
						})
					}
				})
			} else {
				g.prop("checked", false);
			}
			l.addClass('cat-active');
			var n = $(this).next();
			n.bind('mouseenter', function() {
				var e = $(this).closest("li");
				e.attr("data-active", !0)
			}).bind('mouseleave', function() {
				var a = $(this).closest("li");
				a.removeAttr("data-active"), setTimeout(function() {
					a.attr("data-active") || ($(fillID).find(".data-sub").remove(), a.removeClass("cat-active"))
				}, 200)
			});
			g.unbind('click').bind('click', function() {
				var ic = $(this).is(':checked'), gdata = $(this).attr("data");
				if (ic) {
					result_datapool.push(gdata);
					if(isexceed()) {
						ace.addClass('cat-checked');
					} else {
						$(this).attr("checked",false);
						result_datapool.splice($.inArray(gdata,result_datapool),1);
					}
				} else {
					result_datapool.splice($.inArray(gdata,result_datapool),1);
					ace.removeClass('cat-checked');
				}
				copyitem();
			});
		};
	});
	// 复制选中项目
	function copyitem() {
		var resulthtm = '';
		$.each(result_datapool, function(indexrd,valrd) {
			var result_data_array = valrd.split(",");
			resulthtm += '<span data-code="'+result_data_array[0]+','+result_data_array[1]+'">'+result_data_array[1]+'<i class="data-icon data-icon-close"></i></span>';
		});
		$(resultlist).html(resulthtm);
		var spi = $(resultlist + " span");
		spi.unbind().bind('click', function() {
			var spidata = $(this).attr("data-code"),
				apid_array = splitdata(spidata);
			result_datapool.splice($.inArray(spidata,result_datapool),1);
			var catchecked_array = $(fillID + " a.cat-checked");
			$.each(catchecked_array, function() {
				var catcobj = $(this), catcheckedid_array = splitdata(catcobj.attr("data"));
				if(apid_array[0]==catcheckedid_array[0] && apid_array[1]==0){
					catcobj.removeClass('cat-checked');
					catcobj.next().find("input").attr("checked", false);
				} else if (apid_array[0]==catcheckedid_array[0] && apid_array[1]!=0) {
					var rdanum = 0;
					$.each(result_datapool, function(indexdp,valdp) {
						var rdatapool_array = splitdata(valdp);
						if (apid_array[0]==rdatapool_array[0] && apid_array[1]!=0) {
							rdanum ++;
						}
					});
					if (rdanum <= 0) {
						catcobj.removeClass('cat-checked');
					}
				}
			});
			copyitem();
		});
		$("#arscity").html(result_datapool.length);
		if (result_datapool.length > 0) {
			$(".cla").show();
			$(".cla").click(function() {
				result_datapool.splice(0,result_datapool.length);
				var catchecked_array = $(fillID + " a.cat-checked");
				$.each(catchecked_array, function(index, val) {
					$(this).removeClass('cat-checked');
				});
				copyitem();
			})
		} else {
			$(".cla").hide();
		}
	}
	// 分割data 返回数组
	function splitdata(arr) {
		if (arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		};
	}
	// 判断选择数量是否超出
	function isexceed() {
		if(result_datapool.length > 3) {
			alert("最多可选3个");
			return false;
		} else {
			return true;
		}
	}
	// 复制选中 赋值
	function copy_selected() {
		var selectedhtm = "";
		var selectedid_array = new Array();
		var selectedcn_array = new Array();
		for (var i = 0; i < result_datapool.length; i++) {
			if (result_datapool[i]) {
				var data_array = result_datapool[i].split(",");
				selectedid_array[i]=data_array[0];
				selectedcn_array[i]=data_array[1];
			};
		};
		$("#cityText").html(selectedcn_array.join(","));
		$(resulthidId).val(selectedid_array.join(","));
		$("#district_cn").val(selectedcn_array.join(","));
		$("#cityText").parent().find('.input_text_200_bg').removeClass('error');
		$("#cityText").parent().next().find('.error').hide();
	}
	// 点击确定
	$("#ct-selector-save").click(function() {
		copy_selected();
		$('.aui_outer').hide();
		$(".ucc-default").removeClass('aui_is_show');
	});
}
/*职位亮点选择弹出层填充数据*/
function tag_filldata(fillID, data_resources, showID, resultlist, resultshowID, resultID, dir) {
	var tradhtm = '';
	var sourse_length = parseInt(data_resources.length);
	var rows = 0;
	var subscriptnum = 0;
	if((sourse_length%4) == 0) {
		rows = sourse_length / 4;
	} else {
		rows = (sourse_length / 4) + 1;
	}
	for (var i = 0; i < rows; i++) {
		tradhtm += '<tr>';
		for (var j = 0; j < 4; j++) {
			if (data_resources[subscriptnum]) {
				var trad_array = data_resources[subscriptnum].split(",");
				tradhtm += '<td><label><input type="checkbox" data="'+trad_array[0]+','+trad_array[1]+'" name="item-list" class="input-checkbox">'+trad_array[1]+'</label></td>';
				subscriptnum ++;
			};
		};
		tradhtm += '</tr>';
	};
	$(fillID).html(tradhtm);
	// 恢复选中
	if ($(resultID).val().length > 0) {
		var idarray = $(resultID).val().split(",");
		$.each(idarray, function(index, val) {
			$(fillID+" :checkbox").unbind().each(function() {
				var boxdata_array = $(this).attr("data").split(",");
				if(val == boxdata_array[0]) {
					$(this).attr('checked',true);
				}
			});
		});
		copyitem();
		var tagotphtm = '';
		var selectedid_array = new Array();
		var selectedcn_array = new Array();
		$(fillID+" :checkbox[checked]").unbind().each(function(index) {
			var data_array = $(this).attr("data").split(",");
			selectedid_array[index]=data_array[0];
			selectedcn_array[index]=data_array[1];
			tagotphtm += '<div class="input_checkbox"><span rel="'+data_array[0]+'">'+data_array[1]+'</span></div>';
		});
		$(".showchecktag").html(tagotphtm);
		$(resultID).val(selectedid_array.join(","));
		$("#tag_cn").val(selectedcn_array.join(","));
		isexceed($(fillID+" :checkbox[checked]").length);
		// 单选
		$(fillID+" label :checkbox").unbind().live("click",function(event) {
			if ($(this).find(":checkbox").attr('checked')) {
				$(this).find(":checkbox").attr('checked',false);
				isexceed($(fillID+" :checkbox[checked]").length);
			} else {
				$(this).find(":checkbox").attr('checked',true);
				isexceed($(fillID+" :checkbox[checked]").length);
			}
		});
	};
	// 单选
	$(fillID+" label :checkbox").unbind().live("click",function(event) {
		if ($(this).find(":checkbox").attr('checked')) {
			$(this).find(":checkbox").attr('checked',false);
			isexceed($(fillID+" :checkbox[checked]").length);
		} else {
			$(this).find(":checkbox").attr('checked',true);
			isexceed($(fillID+" :checkbox[checked]").length);
		}
	});
	// 复制选中项目
	function copyitem() {
		var resulthtm = '';
		$(fillID+" :checkbox[checked]").unbind().each(function(index) {
			var result_data_array = $(this).attr("data").split(",");
			resulthtm += '<span data-code="'+result_data_array[0]+','+result_data_array[1]+'">'+result_data_array[1]+'<i class="data-icon data-icon-close"></i></span>';
		});
		$(resultlist).html(resulthtm);
		var spi = $(resultlist + " span");
		spi.unbind().bind('click', function() {
			var spidata = $(this).attr("data-code"),
				apid_array = splitdata(spidata);
			$(fillID+" :checkbox[checked]").unbind().each(function(index) {
				var result_data_array = $(this).attr("data").split(",");
				if (apid_array[0] == result_data_array[0]) {
					$(this).attr('checked',false);
					isexceed($(fillID+" :checkbox[checked]").length);
				}
			});
			copyitem();
		});
		$("#arstag").html($(fillID+" :checkbox[checked]").length);
		if ($(fillID+" :checkbox[checked]").length > 0) {
			$(".cla").show();
			$(".cla").click(function() {
				$(fillID+" :checkbox").unbind().each(function(index) {
					$(this).attr('disabled',false);
					$(this).attr('checked',false);
				});
				copyitem();
			})
		} else {
			$(".cla").hide();
		}
	}
	// 复制选中 赋值
	function copy_selected() {
		var selectedid_array = new Array();
		var selectedcn_array = new Array();
		$(fillID+" :checkbox[checked]").unbind().each(function(index) {
			var data_array = $(this).attr("data").split(",");
			selectedid_array[index]=data_array[0];
			selectedcn_array[index]=data_array[1];
		});
		$(resultID).val(selectedid_array.join(","));
		$("#tag_cn").val(selectedcn_array.join(","));
	}
	// 分割data 返回数组
	function splitdata(arr) {
		if(arr) {
			var arrs_array = arr.split(","),
				arrid_array = arrs_array[0].split(".");
			return arrid_array;
		}
	}
	/*判断选中的数量是否超出最大限制*/
	function isexceed(num) {
		if(num >= 5) {
			$(fillID+" :checkbox").each(function(index, el) {
				if (!$(this).attr('checked')) {
					$(this).attr('disabled',"disabled");
				}
			});
		} else {
			$(fillID+" :checkbox").each(function(index, el) {
				$(this).attr('disabled',false);
			});
		}
		copyitem();
	}
	// 点击确定
	$("#tag-selector-save").click(function() {
		copy_selected();
		var tag = $(resultID).val();
		var tag_cn=$("#tag_cn").val();
		var pid =$("#tag_cn").attr('pid');
		// 点击确定保存特长标签
		$.post('?act=ajax_save_tag', {"tag":tag,"tag_cn":tag_cn,"pid":pid}, function(data)
		{
			if (data == "ok") 
			{
				var tagotphtm = '';
				$(fillID+" :checkbox[checked]").unbind().each(function(index) {
					var data_array = $(this).attr("data").split(",");
					tagotphtm += '<div class="input_checkbox"><span rel="'+data_array[0]+'">'+data_array[1]+'</span></div>';
				});
				$(".showchecktag").html(tagotphtm);
				$('.aui_outer').hide();
				$(".ucc-default").removeClass('aui_is_show');
			}
			else
			{
				dialog({
                    title: '系统提示',
                    content: ""+data+"",
                    width:300
                }).show();
			}
		});
	});
}
// 一些js的集合
function allaround(dir) {
	// 显示下拉
	$(".ucc-default").click(function() {
		if (!$(this).hasClass('aui_is_show')) {
			$('.aui_outer').hide();
			$(".ucc-default").removeClass('aui_is_show');
			$(this).addClass('aui_is_show');
			$(this).parent().find('.aui_outer').show();
		} else {
			$(this).removeClass('aui_is_show');
			$(this).parent().find('.aui_outer').hide();
		}
	});
	$(document).delegate("body", "click", function(e){
		var _con = $(".ucc-default"), _caui = $(".aui_outer");
		if(!_con.is(e.target) && _caui.has(e.target).length === 0){
			$('.aui_outer').hide();
			$(".ucc-default").removeClass('aui_is_show');
		}
	});
	// 关闭下拉
	$(".selector-close").die().live('click', function(event) {
		$('.aui_outer').hide();
		$(".ucc-default").removeClass('aui_is_show');
	});
}