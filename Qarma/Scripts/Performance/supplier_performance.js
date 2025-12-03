// --- 1. KHAI BÁO BIẾN TOÀN CỤC ---
let apiData = null;       // Dữ liệu biểu đồ cũ
let apiReportData = null; // Dữ liệu bảng báo cáo (MỚI THÊM)
let paretoData = null;  // Dữ liệu Pareto (nếu cần)
let apiReportMonths = null;

// --- 2. CẤU HÌNH GOOGLE CHARTS ---
// Load thư viện ngay đầu file
google.charts.load('current', { 'packages': ['corechart', 'bar', 'geochart'] });

// Khi Google Charts tải xong, nó sẽ tự động kích hoạt logic khởi chạy
google.charts.setOnLoadCallback(function () {
    // Chỉ chạy khi DOM đã sẵn sàng (đảm bảo lấy được ID từ Dropdown)
    $(document).ready(function () {
        initPage();
    });
});

// --- 3. HÀM KHỞI TẠO TRANG ---
function initPage() {
    // Bắt sự kiện khi đổi dropdown
    $('#customer-select').change(function () {
        var maKhachHang = $(this).val(); // Lấy giá trị

        if (maKhachHang) {
            // TRƯỜNG HỢP 1: Có chọn khách hàng -> Gọi API
            loadDashboardData(maKhachHang);
            // Table
            loadReportTable(maKhachHang);

            loadParetoData(maKhachHang);

            loadReportMonths(maKhachHang);
        } else {
            // TRƯỜNG HỢP 2: Chọn về mặc định (Rỗng) -> Xóa dữ liệu trên màn hình
            resetDashboard();
        }
    });

    // Mặc định gọi hàm reset để màn hình sạch sẽ khi mới vào
    resetDashboard();

    // Resize chart
    //window.addEventListener('resize', function () {
    //    if (apiData) initDashboard();
    //});
}

// --- HÀM MỚI: DỌN DẸP MÀN HÌNH (RESET) ---
function resetDashboard() {
    // 1. Xóa biến dữ liệu toàn cục
    apiData = null;
    apiReportData = null; // Reset luôn cả biến báo cáo

    // 2. Hàm hỗ trợ set text an toàn (Nếu không tìm thấy ID thì bỏ qua, không lỗi)
    var safeSetText = (id, text) => {
        var el = document.getElementById(id);
        if (el) {
            el.innerText = text;
        }
    };

    // 3. Reset thông báo chính
    // Dùng hàm an toàn thay vì gọi trực tiếp document.getElementById
    safeSetText('product-name', "");

    // 4. Reset các con số KPI
    ['kpi-rej', 'kpi-reins', 'kpi-def', 'kpi-meas'].forEach(prefix => {
        safeSetText(`${prefix}-target`, "-- %");
        safeSetText(`${prefix}-val`, "--");

        // Reset kim đồng hồ về 0
        var fill = document.getElementById(`${prefix}-fill`);
        if (fill) fill.style.transform = 'rotate(-180deg)';
    });

    // 5. Xóa các biểu đồ
    ['chart_distribution', 'chart_severity', 'chart_category', 'chart_pareto'].forEach(id => {
        var el = document.getElementById(id);
        if (el) el.innerHTML = "Chưa có dữ liệu";
    });

    // 6. Reset bản đồ
    var mapContainer = document.getElementById('chart_map');
    if (mapContainer) {
        mapContainer.innerHTML = '<div style="display:flex; justify-content:center; align-items:center; height:100%; color:#888;">Chưa có dữ liệu bản đồ</div>';

        // Xóa instance map cũ nếu có để tránh lỗi khi init lại
        // Kiểm tra kỹ L.DomUtil trước khi gọi
        if (typeof L !== 'undefined' && L.DomUtil && L.DomUtil.get('chart_map')) {
            var mapInstance = L.DomUtil.get('chart_map');
            if (mapInstance._leaflet_id) mapInstance._leaflet_id = null;
        }
    }

    // 7. Reset bảng báo cáo chi tiết
    // Kiểm tra xem jQuery có tìm thấy bảng không trước khi gán HTML
    var tableBody = $('#table-report-body');
    if (tableBody.length > 0) {
        tableBody.html('<tr><td colspan="5" class="text-center py-10 text-gray-400 font-medium"><i class="fa-solid fa-arrow-up"></i><br/>Vui lòng chọn khách hàng để xem báo cáo.</td></tr>');
    }
}

// --- 4. HÀM GỌI API (AJAX) ---
function loadParetoData(customerId) {
    // Xóa dữ liệu cũ
    paretoData = null;
    document.getElementById('chart_category').innerHTML = ""; // Xóa biểu đồ tròn cũ
    document.getElementById('chart_pareto').innerHTML = "";   // Xóa biểu đồ cột cũ

    if (!customerId) return;

    $.ajax({
        url: '/SupplierPerformance/GetParetoData',
        type: 'GET',
        data: { customerId: customerId },
        success: function (dataList) {
            paretoData = dataList;

            if (paretoData && paretoData.length > 0) {
                // Vẽ biểu đồ tròn (Defects Category)
                drawCategory();

                // Vẽ biểu đồ cột (Pareto) - Nếu bạn đã có hàm drawPareto chuẩn
                 drawPareto(); 
            } else {
                document.getElementById('chart_category').innerHTML = '<div style="display:flex;justify-content:center;align-items:center;height:100%;color:#9ca3af;">Không có dữ liệu chi tiết</div>';
            }
        },
        error: function (err) {
            document.getElementById('chart_category').innerHTML = '<div style="color:red;text-align:center;padding-top:20px;">Lỗi tải dữ liệu</div>';
        }
    });
}
function loadDashboardData(customerId) {
    var idToSend = customerId ? customerId : "";
    var nameEl = document.getElementById('product-name');

    $.ajax({
        url: '/SupplierPerformance/GetDashboardData',
        type: 'GET',
        data: { id: idToSend },
        success: function (data) {
            apiData = data;
            //initDashboard();
        },
        error: function (err) {
            console.error("Lỗi tải biểu đồ", err);
        }
    });
}
function loadReportTable(customerId) {
    var idToSend = customerId ? customerId : "";
    var tbody = $('#table-report-body');

    tbody.html('<tr><td colspan="5" class="text-center py-6 text-blue-600 font-semibold">Đang cập nhật số liệu...</td></tr>');

    $.ajax({
        url: '/SupplierPerformance/GetReportData',
        type: 'GET',
        data: { customerId: idToSend },
        success: function (dataList) {
            // 1. LƯU DỮ LIỆU VÀO BIẾN TOÀN CỤC
            apiReportData = dataList;

            // 3. Cập nhật lại KPI (để Defect Rate nhận số liệu mới)
            if (apiReportData) {
                renderKpis();
                drawSeverity();
            }
        },
        error: function (err) {
            console.error("Lỗi tải bảng báo cáo", err);
            tbody.html('<tr><td colspan="5" class="text-center py-4 text-red-500">Không thể tải dữ liệu báo cáo.</td></tr>');
        }
    });
}
function loadReportMonths(customerId) {
    var idToSend = customerId ? customerId : "";
    var nameEl = document.getElementById('product-name');

    $.ajax({
        url: '/SupplierPerformance/GetReportByMonths',
        type: 'GET',
        data: { customerId: idToSend },
        success: function (data) {
            apiReportMonths = data;

            //initDashboard();
            drawDistribution();
        },
        error: function (err) {
            console.error("Lỗi tải biểu đồ", err);
        }
    });
}

// --- 5. HÀM ĐIỀU PHỐI VẼ BIỂU ĐỒ ---
//function initDashboard() {
//    if (!apiData) return;

//    try {
//        renderKpis();
//        drawDistribution();
//        drawLeafletMap();
//        drawSeverity();
//        drawCategory();
//        drawPareto();
//    } catch (e) {
//        console.error("Lỗi khi vẽ biểu đồ:", e);
//    }
//}

// --- 6. CÁC HÀM VẼ CHI TIẾT (Giữ nguyên logic của bạn) ---

// ----- Chưa có dữ liệu -----
function drawLeafletMap() {
    var containerId = 'chart_map';
    var container = document.getElementById(containerId);
    if (container) container.innerHTML = '<div style="display:flex;justify-content:center;align-items:center;height:100%;color:#9ca3af;font-size:12px;">Không có dữ liệu</div>';
    return;

    if (typeof L === 'undefined') { return; }
    var container = L.DomUtil.get('chart_map');
    if (container != null) { container._leaflet_id = null; }

    var map = L.map('chart_map').setView([20, 0], 1);

    L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png', {
        attribution: '&copy; OpenStreetMap &copy; CARTO',
        subdomains: 'abcd',
        maxZoom: 19
    }).addTo(map);

    var greenIcon = new L.Icon({
        iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
        shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
        iconSize: [25, 41], iconAnchor: [12, 41], popupAnchor: [1, -34], shadowSize: [41, 41]
    });

    if (apiData.mapLocations) {
        apiData.mapLocations.forEach(loc => {
            var marker = L.marker([loc.lat, loc.long], { icon: greenIcon }).addTo(map);
            marker.bindPopup(`<div style="text-align:center; color:#1f7a4d;"><b>${loc.name}</b><br>Rate: 90%</div>`);
        });
    }
    setTimeout(() => { map.invalidateSize(); }, 200);
}
// ---------------------------
function renderKpis() {
    if (!apiData || !apiData.productInfo) return;

    // 1. Hàm hỗ trợ: Chỉ gán text nếu thẻ tồn tại
    const safeSetText = (id, text) => {
        const el = document.getElementById(id);
        if (el) {
            el.innerText = text;
        } else {
            console.warn(`CẢNH BÁO: Không tìm thấy thẻ HTML có id="${id}"`);
        }
    };

    // 2. Gán dữ liệu cơ bản

    let totalCount = apiData.productInfo.totalInspections;
    //safeSetText('chart-dist-title', `Inspection Distribution - Total Inspections ${totalCount}`);
    safeSetText('map-title', `Inspection Map - Location rate ${apiData.productInfo.locationRate}`);

    // --- LOGIC MỚI ĐỂ LẤY TỈ LỆ LỖI TỪ REPORT DATA ---
    let defectRateValue = null;
    let defectTarget = 1.4; // Target mặc định hoặc lấy từ apiData cũ

    // Nếu đã có dữ liệu từ API Báo cáo
    if (apiReportData && apiReportData.length > 0) {
        // Lấy dòng đầu tiên (vì nếu chọn 1 KH thì list chỉ có 1 dòng, nếu All thì lấy dòng tổng hoặc xử lý sau)
        // Dữ liệu mẫu: [{"MaKhachHang": "...", "TiLeLoi_PhanTram": 10.62, ...}]
        defectRateValue = apiReportData[0].TiLeLoi_PhanTram;
    } else {
        // Fallback: Nếu chưa có dữ liệu báo cáo, dùng dữ liệu dashboard cũ
        defectRateValue = 0.0;
        //defectTarget = 0.0;
    }

    // Tạo object Defect mới để truyền vào updateGauge
    const defectObj = {
        target: defectTarget,
        current: defectRateValue
    };
    // ------------------------------------------------

    // 3. Xử lý các đồng hồ KPI (Gauge)
    const kpi = apiData.kpis;

    const updateGauge = (targetId, fillId, valId, dataObj) => {
        if (!dataObj) return; // Bỏ qua nếu dữ liệu null

        safeSetText(targetId, `${dataObj.target} %`);

        if (dataObj.current === null) {
            safeSetText(valId, "(Blank)");
        } else {
            safeSetText(valId, `${dataObj.current.toFixed(1)} %`);

            // Xử lý xoay kim đồng hồ (cần check thẻ fillId có tồn tại không)
            let fillEl = document.getElementById(fillId);
            if (fillEl) {
                let percent = dataObj.current > 100 ? 100 : dataObj.current;
                setTimeout(() => {
                    fillEl.style.transform = `rotate(${-180 + (percent * 1.8)}deg)`;
                }, 100);
            }
        }
    };

    if (kpi) {
        updateGauge('kpi-rej-target', 'kpi-rej-fill', 'kpi-rej-val', kpi.rejection);
        updateGauge('kpi-reins-target', 'kpi-reins-fill', 'kpi-reins-val', kpi.reinspection);
        updateGauge('kpi-def-target', 'kpi-def-fill', 'kpi-def-val', defectObj);
        updateGauge('kpi-meas-target', 'kpi-meas-fill', 'kpi-meas-val', kpi.measurement);
    }
}

// ----- Chưa có dữ liệu -----
function drawDistribution() {
    var containerId = 'chart_distribution';
    var container = document.getElementById(containerId);

    if (!apiReportMonths || apiReportMonths.length === 0) {
        if (container) {
            container.innerHTML = '<div style="display:flex;justify-content:center;align-items:center;height:100%;color:#9ca3af;font-size:12px;">Không có dữ liệu 12 tháng</div>';
        }
        return;
    }

    const safeSetText = (id, text) => {
        const el = document.getElementById(id);
        if (el) {
            el.innerText = text;
        } else {
            console.warn(`CẢNH BÁO: Không tìm thấy thẻ HTML có id="${id}"`);
        }
    };
    let totalCount = 0;

    var dataArray = [['Month', 'Inspections', { role: 'annotation' }, 'Defect rate', { role: 'annotation' }]];

    apiReportMonths.forEach(item => {
        totalCount += item.SL_Dat;
        let monthLabel = item.ThoiGian;
        let inspections = item.TongKiem;

        let defectRate = item.TiLeLoi_PhanTram / 100;

        let annotationLine = (item.TiLeLoi_PhanTram > 0) ? item.TiLeLoi_PhanTram.toFixed(2) + '%' : null;

        let annotationBar = inspections > 0 ? inspections.toLocaleString() : null;

        dataArray.push([monthLabel, inspections, annotationBar, defectRate, annotationLine]);
    });
    safeSetText('chart-dist-title', `Inspection Distribution - Total Inspections ${totalCount}`);
    var data = google.visualization.arrayToDataTable(dataArray);

    var options = {
        seriesType: 'bars',
        series: {
            0: { color: '#1f7a4d' },
            1: { type: 'line', targetAxisIndex: 1, color: '#2b3340', pointSize: 4, lineWidth: 2 }
        },
        vAxes: {
            0: { title: 'Inspections', textStyle: { color: '#666', fontSize: 10 }, gridlines: { color: '#f3f4f6', count: 5 } },
            1: { title: 'Tỉ lệ lỗi', textStyle: { color: '#666', fontSize: 10 }, format: '#.##%', gridlines: { color: 'transparent' }, viewWindow: { min: 0 } }
        },
        hAxis: { title: 'Tháng', textStyle: { color: '#666', fontSize: 10 }, gridlines: { color: 'transparent' } },
        legend: { position: 'top', alignment: 'start', textStyle: { fontSize: 11 } },
        annotations: { stem: { color: 'transparent' }, textStyle: { color: '#333', fontSize: 9, bold: true }, boxStyle: { stroke: '#ccc', fillOpacity: 1, rx: 3, ry: 3, fill: '#f9fafb' } },
        bar: { groupWidth: "60%" },
        chartArea: { left: 70, right: 50, top: 40, bottom: 40, width: '100%', height: '100%' },
        backgroundColor: 'transparent'
    };

    new google.visualization.ComboChart(document.getElementById('chart_distribution')).draw(data, options);
}
// ---------------------------
function drawSeverity() {
    // 1. SỬA LẠI NGUỒN DỮ LIỆU: Kiểm tra apiReportData thay vì apiData.severity
    var containerId = 'chart_severity';
    var container = document.getElementById(containerId);

    // --- SỬA LỖI TẠI ĐÂY ---
    // Nếu không có dữ liệu, PHẢI XÓA TRẮNG container thay vì chỉ return
    if (!apiReportData || apiReportData.length === 0) {
        if (container) {
            container.innerHTML = '<div style="display:flex;justify-content:center;align-items:center;height:100%;color:#9ca3af;font-size:12px;">Không có dữ liệu</div>';
        }
        return;
    }

    // 2. Tính tổng các loại lỗi (Cộng dồn các dòng trong bảng báo cáo)
    let sumCritical = 0;
    let sumMajor = 0;
    let sumMinor = 0;

    apiReportData.forEach(item => {
        sumCritical += (item.Critical || 0);
        sumMajor += (item.Major || 0);
        sumMinor += (item.Minor || 0);
    });

    if (sumCritical + sumMajor + sumMinor === 0) {
        container.innerHTML = '<div style="display:flex;justify-content:center;align-items:center;height:100%;color:#9ca3af;font-size:12px;">Dữ liệu lỗi = 0</div>';
        return;
    }

    // 3. Tạo mảng dữ liệu cứng cho 3 loại lỗi này
    // Format: [Label, Value, Style (Màu), Annotation (Số hiển thị)]
    var dataArray = [['Severity', 'Count', { role: 'style' }, { role: 'annotation' }]];

    // Dòng 1: Minor (Màu Xanh Dương)
    dataArray.push(['Lỗi nhẹ', sumMinor, 'color: #3b82f6; stroke-width: 0;', sumMinor.toString()]);

    // Dòng 2: Major (Màu Cam)
    dataArray.push(['Lỗi nặng', sumMajor, 'color: #f97316; stroke-width: 0;', sumMajor.toString()]);

    // Dòng 3: Critical (Màu Đỏ Đậm)
    dataArray.push(['Lỗi nghiêm trọng', sumCritical, 'color: #dc2626; stroke-width: 0;', sumCritical.toString()]);

    // 4. Tìm giá trị lớn nhất để căn chỉnh trục (Tránh biểu đồ bị bẹp nếu số nhỏ)
    let maxVal = Math.max(sumCritical, sumMajor, sumMinor);
    if (maxVal === 0) maxVal = 5;

    // 5. Vẽ biểu đồ (Giữ nguyên cấu hình cũ)
    var data = google.visualization.arrayToDataTable(dataArray);
    var view = new google.visualization.DataView(data);

    view.setColumns([0, 1, 2, 3, {
        type: 'string',
        role: 'annotationTextStyle',
        calc: function (dt, row) {
            // Nếu giá trị = 0 thì hiện số màu xám, ngược lại màu trắng
            return (dt.getValue(row, 1) === 0) ? 'color: #9ca3af; font-weight: bold;' : 'color: #ffffff; font-weight: bold;';
        }
    }]);

    var options = {
        bars: 'horizontal',
        legend: 'none',
        backgroundColor: 'transparent',
        chartArea: { left: 85, top: 15, right: 30, bottom: 45, height: '100%', width: '100%' },
        hAxis: {
            title: 'Defects sum',
            textStyle: { color: '#666', fontSize: 11 },
            gridlines: { color: '#d1d5db', style: 'dotted' },
            viewWindow: { max: maxVal * 1.15 }
        },
        vAxis: {
            title: 'Defect severity',
            textStyle: { color: '#666', fontSize: 11 },
            gridlines: { color: 'transparent' }
        },
        bar: { groupWidth: "70%" },
        annotations: { alwaysOutside: false, textStyle: { fontSize: 11 } }
    };

    new google.visualization.BarChart(document.getElementById('chart_severity')).draw(view, options);
}

function drawCategory() {
    var containerId = 'chart_category';
    var container = document.getElementById(containerId);

    if (!paretoData || paretoData.length === 0) {
        if (container) container.innerHTML = '<div style="display:flex;justify-content:center;align-items:center;height:100%;color:#9ca3af;font-size:12px;">Không có dữ liệu</div>';
        return;
    }

    // 1. Gom nhóm dữ liệu
    let groupedData = {};
    let totalCount = 0;

    paretoData.forEach(item => {
        let name = item.TenLoi || "Khác";
        let count = item.SoLuongLoi || 0;
        if (groupedData[name]) groupedData[name] += count;
        else groupedData[name] = count;
        totalCount += count;
    });

    // 2. Chuyển sang mảng object và sắp xếp
    var dataItems = [];
    for (var key in groupedData) {
        if (groupedData.hasOwnProperty(key)) {
            dataItems.push({ name: key, count: groupedData[key] });
        }
    }
    dataItems.sort((a, b) => b.count - a.count);

    // 3. Chuẩn bị dữ liệu vẽ (THÊM CỘT TOOLTIP)
    // Format: [Label, Value, Tooltip]
    var dataArray = [['Category', 'Count', { type: 'string', role: 'tooltip', p: { html: true } }]];

    // Top 10 + Khác
    let topItems = dataItems.slice(0, 9);
    let otherCount = 0;
    if (dataItems.length > 10) {
        for (let i = 10; i < dataItems.length; i++) {
            otherCount += dataItems[i].count;
        }
    }

    // Mảng màu (để lấy màu chấm tròn trong tooltip cho đúng)
    var colorsArray = ['#1f7a4d', '#2ecc71', '#3498db', '#9b59b6', '#f1c40f', '#e67e22', '#e74c3c', '#95a5a6', '#34495e', '#16a085', '#bdc3c7'];

    // Hàm tạo HTML Tooltip
    const createTooltip = (name, count, total, color) => {
        let percent = ((count / total) * 100).toFixed(1) + '%';
        return `
            <div class="custom-tooltip">
                <div class="tooltip-header">${name}</div>
                <div class="tooltip-body">
                    <div style="display:flex; align-items:center;">
                        <span class="tooltip-dot" style="background-color: ${color};"></span>
                        <span>Số lượng:</span>
                    </div>
                    <span class="tooltip-value" style="color: ${color};">${count}</span>
                </div>
                <div class="tooltip-body" style="margin-top:2px;">
                    <span style="margin-left: 18px; color: #6b7280; font-size: 12px;">Tỷ lệ:</span>
                    <span style="font-weight: 600; font-size: 12px;">${percent}</span>
                </div>
            </div>
        `;
    };

    // Đẩy dữ liệu vào mảng
    topItems.forEach((item, index) => {
        let color = colorsArray[index % colorsArray.length];
        let tooltipHtml = createTooltip(item.name, item.count, totalCount, color);
        dataArray.push([item.name, item.count, tooltipHtml]);
    });

    if (otherCount > 0) {
        let color = colorsArray[topItems.length % colorsArray.length];
        let tooltipHtml = createTooltip("Các lỗi khác", otherCount, totalCount, color);
        dataArray.push(['Các lỗi khác', otherCount, tooltipHtml]);
    }

    var data = google.visualization.arrayToDataTable(dataArray);

    var options = {
        pieHole: 0, // Biểu đồ Donut (có lỗ ở giữa) nhìn hiện đại hơn
        colors: colorsArray,
        legend: {
            position: 'right',
            alignment: 'center',
            textStyle: { fontSize: 11, color: '#555', fontName: 'Inter' },
            title: 'Defect Category',
            titleTextStyle: { fontSize: 12, color: '#333', bold: true, fontName: 'Inter' }
        },
        pieSliceText: 'percentage',
        pieSliceTextStyle: { color: '#ffffff', fontSize: 11, fontName: 'Inter' },

        // Tinh chỉnh vùng vẽ để Legend không bị cắt chữ
        chartArea: { left: 10, top: 20, width: '90%', height: '85%' },

        backgroundColor: 'transparent',

        tooltip: { isHtml: true },
        focusTarget: 'category'
    };

    new google.visualization.PieChart(container).draw(data, options);
}

function drawPareto() {
    var containerId = 'chart_pareto';
    var container = document.getElementById(containerId);

    // 1. Kiểm tra dữ liệu
    if (!paretoData || paretoData.length === 0) {
        if (container) container.innerHTML = '<div style="display:flex;justify-content:center;align-items:center;height:100%;color:#9ca3af;font-size:12px;">Không có dữ liệu</div>';
        return;
    }

    // 2. Gom nhóm dữ liệu theo Tên Lỗi
    let groupedData = {};
    let totalErrors = 0;

    paretoData.forEach(item => {
        let name = item.TenLoi || "Khác";
        let count = item.SoLuongLoi || 0;

        if (groupedData[name]) {
            groupedData[name] += count;
        } else {
            groupedData[name] = count;
        }
        totalErrors += count;
    });

    // Chuyển sang mảng object để dễ xử lý
    var dataItems = [];
    for (var key in groupedData) {
        if (groupedData.hasOwnProperty(key)) {
            dataItems.push({ name: key, count: groupedData[key] });
        }
    }

    // 3. Sắp xếp giảm dần (QUAN TRỌNG CHO PARETO)
    dataItems.sort((a, b) => b.count - a.count);

    // Chỉ lấy Top 15 lỗi để biểu đồ không quá dày đặc
    let displayItems = dataItems.slice(0, 15);

    // 4. Chuẩn bị dữ liệu vẽ
    // --- THAY ĐỔI 1: Thêm cột annotation cho cột Bar ---
    // Format cũ: [Label, Bar Value, Style Bar, Line Value (%), Annotation Line (%)]
    // Format mới: [Label, Bar Value, Annotation Bar, Style Bar, Line Value (%), Annotation Line (%)]
    var dataArray = [['Type', 'Số lượng', { role: 'annotation' }, { role: 'style' }, 'Tỷ lệ tích lũy %', { role: 'annotation' }]];

    const mainColor = '#1f7a4d'; // Màu cột chính
    // const lastColor = '#e5e7eb'; // Màu cột cuối (nếu muốn làm nhạt đi) - hiện tại không dùng

    let runningSum = 0; // Tổng tích lũy để tính % đường line

    displayItems.forEach((item, index) => {
        runningSum += item.count;
        let cumulativePercent = (runningSum / totalErrors); // Tỷ lệ tích lũy (0.1, 0.5, 1.0)

        // Format hiển thị % trên đường Line (VD: 80%)
        let percentAnnotation = (cumulativePercent * 100).toFixed(0) + '%';

        // Màu sắc: ta dùng 1 màu cho thống nhất
        let color = `color: ${mainColor}; stroke-width: 0;`;

        // --- THAY ĐỔI 2: Đẩy thêm giá trị item.count.toString() vào mảng dữ liệu ---
        dataArray.push([
            item.name,           // Tên lỗi (Trục hoành)
            item.count,          // Số lượng (Cột Bar)
            item.count.toString(), // <--- Annotation: Hiển thị số lượng trên đầu cột
            color,               // Màu cột
            cumulativePercent,   // Giá trị đường Line (0.0 - 1.0)
            percentAnnotation    // Nhãn hiển thị trên đường Line (ô % nhỏ)
        ]);
    });

    var data = google.visualization.arrayToDataTable(dataArray);

    var options = {
        seriesType: 'bars',
        backgroundColor: 'transparent',

        // Cấu hình 2 trục Y (Trục trái: Số lượng, Trục phải: %)
        series: {
            0: { color: '#1f7a4d' }, // Series 0: Cột (Bar)
            1: {
                type: 'line',
                targetAxisIndex: 1, // Gắn vào trục phải
                color: '#374151',
                pointSize: 4,
                lineWidth: 2
            }
        },

        vAxes: {
            0: {
                title: 'Số lượng lỗi',
                textStyle: { fontSize: 10, color: '#666' },
                gridlines: { color: '#d1d5db', style: 'dotted', count: 5 }
            },
            1: {
                title: 'Tỷ lệ tích lũy %',
                textStyle: { fontSize: 10, color: '#666' },
                format: '#%', // Format trục thành %
                gridlines: { color: 'transparent' },
                viewWindow: { min: 0, max: 1.05 } // Max hơn 1 chút để đỉnh 100% không bị cắt
            }
        },

        hAxis: {
            title: 'Loại lỗi',
            textStyle: { fontSize: 9, color: '#666' },
            gridlines: { color: 'transparent' },
            slantedText: true, // Nghiêng chữ nếu tên dài
            slantedTextAngle: 45
        },

        legend: { position: 'top', alignment: 'start', textStyle: { fontSize: 11 } },

        annotations: {
            stem: { color: 'transparent' },
            textStyle: { color: '#333', fontSize: 9, bold: true },
            boxStyle: { stroke: '#ccc', fillOpacity: 1, rx: 3, ry: 3, fill: '#f9fafb' }
        },

        bar: { groupWidth: "70%" },
        chartArea: { left: 50, right: 50, top: 30, bottom: 80, width: '100%', height: '100%' }
    };

    new google.visualization.ComboChart(container).draw(data, options);
}
