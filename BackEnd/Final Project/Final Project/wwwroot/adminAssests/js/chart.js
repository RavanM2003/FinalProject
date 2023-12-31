$(function() {
  'use strict';
var names = [];
var salaries = [];

var workers = document.querySelectorAll(".name-worker")
workers.forEach(element => {
  names.push(element.value)
});
var salariesArr = document.querySelectorAll(".salary-worker")
salariesArr.forEach(element => {
  salaries.push(element.value)
});

var data = {
    labels:names,
    datasets: [{
        label: 'Salary',
        data: salaries,
        backgroundColor: [
          'rgba(255, 102, 102, 0.5)',
          'rgba(102, 204, 102, 0.5)',
          'rgba(102, 102, 255, 0.5)',
          'rgba(255, 204, 102, 0.5)',
          'rgba(255, 102, 255, 0.5)',
          'rgba(102, 255, 255, 0.5)',
          'rgba(204, 102, 255, 0.5)',
          'rgba(255, 153, 51, 0.5)',
          'rgba(51, 153, 102, 0.5)',
          'rgba(51, 102, 153, 0.5)',
          'rgba(255, 102, 51, 0.5)',
          'rgba(51, 204, 204, 0.5)'
        ],
        borderColor: [
          'rgba(255, 102, 102, 1)',
          'rgba(102, 204, 102, 1)',
          'rgba(102, 102, 255, 1)',
          'rgba(255, 204, 102, 1)',
          'rgba(255, 102, 255, 1)',
          'rgba(102, 255, 255, 1)',
          'rgba(204, 102, 255, 1)',
          'rgba(255, 153, 51, 1)',
          'rgba(51, 153, 102, 1)',
          'rgba(51, 102, 153, 1)',
          'rgba(255, 102, 51, 1)',
          'rgba(51, 204, 204, 1)'
        ],
        borderWidth: 2,
        fill: false,
    }],
};
  var options = {
    scales: {
      yAxes: [{
        ticks: {
          beginAtZero: true
        }
      }]
    },
    legend: {
      display: false
    },
    elements: {
      point: {
        radius: 10
      }
    },
  };
  var categoryNames = [];
  var categoryNameArr = document.querySelectorAll(".name-category")
  categoryNameArr.forEach(element => {
    categoryNames.push(element.value)
  });
  var total = [];
  var totalArr = document.querySelectorAll(".total-category")
  totalArr.forEach(element => {
    total.push(element.value)
  });
  var doughnutPieData = {
    datasets: [{
      data: total,
      backgroundColor: [
        'rgba(255, 99, 132, 0.5)',
        'rgba(54, 162, 235, 0.5)',
        'rgba(255, 206, 86, 0.5)',
        'rgba(255, 102, 102, 0.5)',
        'rgba(102, 204, 102, 0.5)',
        'rgba(102, 102, 255, 0.5)',
        'rgba(255, 204, 102, 0.5)',
        'rgba(255, 102, 255, 0.5)',
        'rgba(102, 255, 255, 0.5)',
        'rgba(204, 102, 255, 0.5)',
        'rgba(255, 153, 51, 0.5)',
        'rgba(51, 153, 102, 0.5)',
        'rgba(51, 102, 153, 0.5)',
        'rgba(255, 102, 51, 0.5)',
        'rgba(51, 204, 204, 0.5)'
      ],
      borderColor: [
        'rgba(255,99,132,1)',
        'rgba(54, 162, 235, 1)',
        'rgba(255, 206, 86, 1)',
        'rgba(255, 102, 102, 1)',
        'rgba(102, 204, 102, 1)',
        'rgba(102, 102, 255, 1)',
        'rgba(255, 204, 102, 1)',
        'rgba(255, 102, 255, 1)',
        'rgba(102, 255, 255, 1)',
        'rgba(204, 102, 255, 1)',
        'rgba(255, 153, 51, 1)',
        'rgba(51, 153, 102, 1)',
        'rgba(51, 102, 153, 1)',
        'rgba(255, 102, 51, 1)',
        'rgba(51, 204, 204, 1)'
      ],
    }],

    labels: categoryNames
  };
  var doughnutPieOptions = {
    responsive: true,
    animation: {
      animateScale: true,
      animateRotate: true
    }
  };
  if ($("#doughnutChart").length) {
    var doughnutChartCanvas = $("#doughnutChart").get(0).getContext("2d");
    var doughnutChart = new Chart(doughnutChartCanvas, {
      type: 'doughnut',
      data: doughnutPieData,
      options: doughnutPieOptions
    });
  }
  if ($("#barChart").length) {
    var barChartCanvas = $("#barChart").get(0).getContext("2d");
    var barChart = new Chart(barChartCanvas, {
      type: 'bar',
      data: data,
      options: options
    });
  }
  var scrollableDiv = document.getElementById("scrollableDiv");
  scrollableDiv.scrollTop = scrollableDiv.scrollHeight;
});