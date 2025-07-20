<script lang="ts">
  export let value: number = 0;       // downloaded bytes
  export let total: number = 1;       // total bytes
  
  $: percent = total > 0 ? Math.min((value / total) * 100, 100) : 0;
</script>

<div class={'progress-bar ' + ($$restProps.class || '')}>
  <div class="progress-bar-value" style="width: {percent}%; height: 100%;"></div>
</div>