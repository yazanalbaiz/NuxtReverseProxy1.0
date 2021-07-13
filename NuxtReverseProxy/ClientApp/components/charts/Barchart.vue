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
          categories: [1920, 1930, 1940, 1994, 1995, 1996, 1997],
        },
      },
      series: [
        {
          name: 'Cars',
          data: [30, 45, 50, 49, 60, 70, 91],
        },
        {
          name: 'Boats',
          data: [25, 50, 75, 45, 30, 35, 100],
        },
      ],
    };
  },
};
</script>