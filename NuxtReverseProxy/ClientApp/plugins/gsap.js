import { gsap } from 'gsap';
import { ScrollTrigger } from 'gsap/ScrollTrigger';
export default function (_, inject) {
  gsap.registerPlugin(ScrollTrigger);
  // Will be available in the components as this.$myService
  inject('gsap', gsap);
}
