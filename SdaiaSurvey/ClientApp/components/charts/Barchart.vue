<template>
  <div class="d-flex justify-center">
    <client-only>
      <ApexChart
        width="500"
        type="bar"
        :options="options"
        :series="series"
      ></ApexChart
    ></client-only>
  </div>
</template>

<script>
import arabicApexChart from 'apexcharts/dist/locales/ar.json';
export default {
  data: function () {
    return {
      options: {
        chart: {
          locales: [arabicApexChart],
          defaultLocale: 'ar',
          id: 'vuechart-example', 
          fontFamily: 'system-ui, Helvetica, Arial, sans-serif',
        },
        xaxis: {
          categories: ["اهلا", 1992, 1993, 1994, 1995, 1996, 1997, 1998],
        },
      },
      series: [
        {
          name: 'series-1',
          data: ["و سهلا", 45, 50, 49, 60, 70, 91],
        },
      ],
    };
  },
};
</script>